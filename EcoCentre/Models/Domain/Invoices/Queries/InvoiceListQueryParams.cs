using System;
using EcoCentre.Models.Domain.Common;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
    public class InvoiceListQueryParams
    {
        public string Id { get; set; }
        public bool Deleted { get; set; }
        public string UserId { get; set; }
        public int Page { get; set; }
        public string Term { get; set; }
        public bool CurrentYear { get; set; }
        public SortDir SortDir { get; set; }
        public InvoiceSortTerm SortBy { get; set; }
        public InvoiceSearchBy Type { get; set; }
        public DateTime? TermFrom { get; set; }
        public DateTime? TermTo { get; set; }
        public string CenterName { get; set; }

        public int? PageSize { get; set; }
    }
}