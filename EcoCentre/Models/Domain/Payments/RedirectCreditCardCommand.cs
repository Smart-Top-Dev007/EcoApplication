using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.MoneticoPayments;
using FluentValidation;
using FluentValidation.Results;

namespace EcoCentre.Models.Domain.Payments
{
	public class RedirectCreditCardCommand
	{
		private readonly HttpContextBase _httpContext;
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly AuthenticationContext _context;
		private readonly PaymentProcessor _paymentProcessor;
		private readonly Repository<PaymentInitialization> _paymentInitializationRepository;
		private readonly Repository<MoneticoConfiguration> _moneticoConfigurationRepository;

		public RedirectCreditCardCommand(
			HttpContextBase httpContext,
			Repository<Invoice> invoiceRepository,
			AuthenticationContext context,
			PaymentProcessor paymentProcessor,
			Repository<PaymentInitialization> paymentInitializationRepository,
			Repository<MoneticoConfiguration> moneticoConfigurationRepository)
		{
			_httpContext = httpContext;
			_invoiceRepository = invoiceRepository;
			_context = context;
			_paymentProcessor = paymentProcessor;
			_paymentInitializationRepository = paymentInitializationRepository;
			_moneticoConfigurationRepository = moneticoConfigurationRepository;
		}
		public string InvoiceId { get; set; }
		public PaymentRedirectViewModel Execute()
		{

			var invoice = _invoiceRepository.FindOne(InvoiceId);

			if (invoice == null)
			{
				throw new NotFoundException(typeof(Invoice), InvoiceId);
			}

			if (invoice.Payment != null)
			{
				throw new ValidationException(new ValidationFailure(nameof(InvoiceId), $"Invoice {InvoiceId} is already paid.").AsList());
			}

			var config = _moneticoConfigurationRepository.Query.FirstOrDefault();
			if (config == null)
			{
				throw new DomainException("Payment configuration not present. Contact administrator to set it up.");
			}

			var paymentInitialization = new PaymentInitialization
			{
				Reference = Guid.NewGuid().ToString("N").Substring(0, 12),
				InvoiceId = InvoiceId,
				Responses = new List<string>(),
				DateInserted = DateTime.UtcNow,
				User = new UserInfo(_context.User)
			};
			_paymentInitializationRepository.Save(paymentInitialization);

			var url = _httpContext.Request.Url;
			var paymentRequest = new PaymentRequest
			{
				Amount = invoice.AmountIncludingTaxes,
				RedirectUrl = $"{url.Scheme}://{url.Authority}/#invoice/show/{InvoiceId}",
				RedirectSuccessUrl = $"{url.Scheme}://{url.Authority}/#invoice/show/{InvoiceId}",
				RedirectErrorUrl = $"{url.Scheme}://{url.Authority}/#invoice/show/{InvoiceId}",
				Date = DateTime.UtcNow,
				Reference = paymentInitialization.Reference,
				FreeText = invoice.InvoiceNo
			};

			return new PaymentRedirectViewModel
			{
				Values = _paymentProcessor.CreateRedirect(paymentRequest, config),
				RedirectUrl = config.RedirectUrl()
			};
		}
	}
}