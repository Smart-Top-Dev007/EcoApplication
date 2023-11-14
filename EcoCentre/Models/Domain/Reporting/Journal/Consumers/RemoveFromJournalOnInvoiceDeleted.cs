using System.Linq;
using EcoCentre.Models.Domain.Invoices.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.Reporting.Journal.Consumers
{
	public class RemoveFromJournalOnInvoiceDeleted : Consumes<InvoiceDeletedEvent>.All
	{
		private readonly Repository<InvoiceJournal> _journalRepository;

		public RemoveFromJournalOnInvoiceDeleted(Repository<InvoiceJournal> journalRepository )
		{
			_journalRepository = journalRepository;
		}

		public void Consume(InvoiceDeletedEvent message)
		{
			
			var journal = _journalRepository.Query.SingleOrDefault(x=>x.InvoiceId == message.InvoiceId);
			_journalRepository.Remove(journal);
			
		}
	}
}