using System;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;
using FluentValidation;
using FluentValidation.Results;

namespace EcoCentre.Models.Domain.Payments
{
	public class PayWithCashCommand
	{
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly AuthenticationContext _context;

		public PayWithCashCommand(Repository<Invoice> invoiceRepository, AuthenticationContext context)
		{
			_invoiceRepository = invoiceRepository;
			_context = context;
		}
		public string InvoiceId { get; set; }

		public Payment Execute()
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

			var payment = new Payment
			{
				PaymentMethod = PaymentMethod.Cash,
				ProcessedByUser = new UserInfo(_context.User),
				DateProcessed = DateTime.UtcNow
			};

			invoice.Payment = payment;

			_invoiceRepository.Save(invoice);

			return payment;
		}
	}
}