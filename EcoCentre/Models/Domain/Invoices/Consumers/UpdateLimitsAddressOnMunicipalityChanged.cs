using System;
using System.Linq;
using EcoCentre.Models.Domain.Invoices.Events;
using EcoCentre.Models.Domain.Limits;
using MassTransit;

namespace EcoCentre.Models.Domain.Invoices.Consumers
{
    public class UpdateInvoceVisitNumber : Consumes<InvoiceAddedEvent>.All, Consumes<InvoiceDeletedEvent>.All
	{
        
		private readonly Repository<Invoice> _invoiceRepo;
		private readonly Repository<DeletedInvoice> _deletedInvoiceRepo;

		public UpdateInvoceVisitNumber(Repository<Invoice> invoiceRepo, Repository<DeletedInvoice> deletedInvoiceRepo)
		{
			_invoiceRepo = invoiceRepo;
			_deletedInvoiceRepo = deletedInvoiceRepo;
		}

        
		public void Consume(InvoiceAddedEvent message)
		{
			var invoice = _invoiceRepo.FindOne(message.InvoiceId);
			if (invoice == null)
			{
				return;
			}
			Update(invoice.ClientId);
		}

		public void Consume(InvoiceDeletedEvent message)
		{
			var invoice = _deletedInvoiceRepo.Query.FirstOrDefault(x => x.Invoice.Id == message.InvoiceId);
			if (invoice == null)
			{
				return;
			}
			Update(invoice.Invoice.ClientId);
		}

		private void Update(string clientId)
		{
			var thisYear = new DateTime(DateTime.Now.Year, 1, 1);

			var invoices = _invoiceRepo.Query.Where(x => x.ClientId == clientId && x.CreatedAt >= thisYear)
				.OrderBy(x => x.CreatedAt)
				.ToList();

			var visitNumber = 0;
			foreach (var invoice in invoices)
			{
				visitNumber++;
				if (invoice.VisitNumber != visitNumber)
				{
					invoice.VisitNumber = visitNumber;
					_invoiceRepo.Save(invoice);
				}
			}

		}
	}
}