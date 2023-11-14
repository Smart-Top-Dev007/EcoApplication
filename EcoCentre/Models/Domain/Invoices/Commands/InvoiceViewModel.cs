using System.Collections.Generic;

namespace EcoCentre.Models.Domain.Invoices.Commands
{
    public class InvoiceViewModel
    {
        public string ClientId { get; set; }
        public string EmployeeName { get; set; }
        public IList<InvoiceMaterialViewModel> Materials { get; set; }
        public IList<InvoiceAttachmentViewModel> Attachments { get; set; }
        public string Comment { get; set; }
	    public IList<InvoiceGiveawayItemViewModel> GiveawayItems { get; set; }
    }
}