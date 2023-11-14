using System.Linq;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Events;
using MassTransit;
using NLog;

namespace EcoCentre.Models.Domain.Limits.Consumers
{
    public class UpdateLimitsOnInvoiceDeleted : Consumes<InvoiceDeletedEvent>.All
    {
	    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Repository<LimitStatus> _limitsRepository;
        private readonly Repository<DeletedInvoice> _deletedInvoiceRepository;
        
        public UpdateLimitsOnInvoiceDeleted(Repository<LimitStatus> limitsRepository, Repository<DeletedInvoice> invoiceRepository)
        {
            _limitsRepository = limitsRepository;
	        _deletedInvoiceRepository = invoiceRepository;            
		}

        public void Consume(InvoiceDeletedEvent message)
        {
	        var invoice = _deletedInvoiceRepository.Query.FirstOrDefault(x=> x.Invoice.Id == message.InvoiceId)?.Invoice;

			var limits = _limitsRepository.Query.FirstOrDefault(x => x.Address.Id == invoice.Address.Id);
			if (limits == null)
            {
	            Logger.Error("Can't find limits for year {invoiceYear} for invoice {invoiceId}", invoice.CreatedAt.Year, invoice.InvoiceNo);
				return;
            }
            limits.RemoveFromLimits(invoice);
            _limitsRepository.Save(limits);
        }

    }
}