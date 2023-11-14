using System.Collections.Generic;
using EcoCentre.Models.Queries;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
    public class InvoiceListQueryResult
    {
        public IEnumerable<InvoiceDetails> Invoices { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
    }
}