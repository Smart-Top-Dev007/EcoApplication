using System;
using System.Linq;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.MoneticoPayments;
using FluentValidation;
using FluentValidation.Results;
using NLog;

namespace EcoCentre.Models.Domain.Payments
{
	public class PaymentResponseCommand
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Repository<Invoice> _invoiceRepository;
		private readonly Repository<PaymentInitialization> _paymentAuthorizationRequestRepository;
		private readonly PaymentResponseValidator _paymentValidator;
		private readonly Repository<MoneticoConfiguration> _moneticoConfigurationRepository;

		public PaymentResponseCommand(
			Repository<Invoice> invoiceRepository,
			Repository<PaymentInitialization> paymentAuthorizationRequestRepository,
			PaymentResponseValidator paymentValidator,
			Repository<MoneticoConfiguration> moneticoConfigurationRepository)
		{
			_invoiceRepository = invoiceRepository;
			_paymentAuthorizationRequestRepository = paymentAuthorizationRequestRepository;
			_paymentValidator = paymentValidator;
			_moneticoConfigurationRepository = moneticoConfigurationRepository;
		}
		
		public string Execute(string body)
		{
			Logger.Info("Payment response: "+body);

			var config = _moneticoConfigurationRepository.Query.FirstOrDefault();
			if (config == null)
			{
				throw new DomainException("Payment configuration not present. Contact administrator to set it up.");
			}

			var result = _paymentValidator.ValidateResponse(body, config.Key);

			if (!result.IsValid)
			{
				Logger.Warn($"Incorrect MAC! Body: {body}");
				return result.Response;
			}
			
			var paymentRequest = _paymentAuthorizationRequestRepository.Query.FirstOrDefault(x => x.Reference == result.Reference);
			if (paymentRequest == null)
			{
				throw new NotFoundException($"Payment request '{result.Reference}' not found.");
			}
			var invoiceId = paymentRequest.InvoiceId;
			var invoice = _invoiceRepository.FindOne(invoiceId);

			if (invoice == null)
			{
				throw new NotFoundException(typeof(Invoice), invoiceId);
			}

			if (invoice.Payment != null)
			{
				throw new ValidationException(new ValidationFailure(nameof(invoiceId), $"Invoice {invoiceId} is already paid.").AsList());
			}

			if (result.PaymentStatus == PaymentStatus.Approved ||
			    result.PaymentStatus == PaymentStatus.ApprovedTest)
			{
				var payment = new Payment
				{
					MoneticoPaymentDate = result.Date,
					Amount = result.Amount,
					Reference = result.Reference,
					ProcessedByUser = paymentRequest.User,
					DateProcessed = DateTime.UtcNow,
					IsTestPayment = result.PaymentStatus == PaymentStatus.ApprovedTest
				};

				payment.PaymentMethod = GetPaymentMethod(result);

				invoice.Payment = payment;

				_invoiceRepository.Save(invoice);
			}

			paymentRequest.Responses.Add(body);
			_paymentAuthorizationRequestRepository.Save(paymentRequest);

			return result.Response;
		}

		private static PaymentMethod GetPaymentMethod(PaymentValidationResult result)
		{
			switch (result.CardType)
			{
				case CardType.Amex:
					return PaymentMethod.AmexCreditCard;
				case CardType.MasterCard:
					return PaymentMethod.MasterCardCreditCard;
				case CardType.Visa:
					return PaymentMethod.VisaCreditCard;
			}

			return PaymentMethod.CreditCard;
		}
	}
}