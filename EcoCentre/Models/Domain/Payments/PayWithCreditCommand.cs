using System;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;
using FluentValidation;
using FluentValidation.Results;

namespace EcoCentre.Models.Domain.Payments
{
	public class PayWithCreditCommand
	{
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly Repository<Client> _clientRepository;
		private readonly AuthenticationContext _context;

		public PayWithCreditCommand(Repository<Invoice> invoiceRepository, Repository<Client> clientRepository, AuthenticationContext context)
		{
			_invoiceRepository = invoiceRepository;
			_clientRepository = clientRepository;
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

			var client = _clientRepository.FindOne(invoice.ClientId);
			if (client.AllowCredit == false)
			{
				throw new ValidationException(new ValidationFailure(nameof(InvoiceId), $"Invoice {InvoiceId} can't be paid in credit, because user is has this option enabled.").AsList());
			}


			var payment = new Payment
			{
				PaymentMethod = PaymentMethod.Credit,
				ProcessedByUser = new UserInfo(_context.User),
				DateProcessed = DateTime.UtcNow
			};

			invoice.Payment = payment;

			_invoiceRepository.Save(invoice);

			return payment;
		}
	}
}