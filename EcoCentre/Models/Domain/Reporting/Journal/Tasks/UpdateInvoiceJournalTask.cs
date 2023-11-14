using System.Linq;
using System.Threading;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Events;
using EcoCentre.Models.Domain.Reporting.Journal.Consumers;

namespace EcoCentre.Models.Domain.Reporting.Journal.Tasks
{
    public class UpdateInvoiceJournalTask : AsyncAdminTask
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<InvoiceJournal> _journalRepository;
        private readonly AddToJournalOnInvoiceAdded _addToJournalOnInvoiceAdded;

        public UpdateInvoiceJournalTask(Repository<Invoice> invoiceRepository, 
            Repository<InvoiceJournal> journalRepository,
            AddToJournalOnInvoiceAdded addToJournalOnInvoiceAdded) 
        {
            _invoiceRepository = invoiceRepository;
            _journalRepository = journalRepository;
            _addToJournalOnInvoiceAdded = addToJournalOnInvoiceAdded;
        }


        protected override void DoWork()
        {
            _journalRepository.RemoveAll();
            var ids = _invoiceRepository.Query.Select(x => x.Id).ToList();
            var items = ids.Count;
            var index = 0;
            Thread.SpinWait(5000);
            foreach (var id in ids)
            {
                index++;
                _addToJournalOnInvoiceAdded.Consume(new InvoiceAddedEvent {InvoiceId = id});
                Progress = (decimal)index/items;
            }
        }

        
    }
}