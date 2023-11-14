using EcoCentre.Models.Domain.Payments;

namespace EcoCentre.Models.Domain.Reporting.Journal
{
    public class JournalReportResultRow
    {
        public string Id { get; set; }
        public string InvoiceNo { get; set; } 
        public string Date { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientName { get; set; } 
        public string Type { get; set; } 
        public string Address { get; set; } 
        public string City { get; set; }
        public string Materials { get; set; }
        public string InvoiceId { get; set; }
        public string ClientId { get; set; }
	    public decimal AmountIncludingTaxes { get; set; }
	    public string PaymentMethod { get; set; }
	    public bool? IsTestPayment { get; set; }
    }
}