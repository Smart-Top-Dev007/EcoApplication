using System;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Queries;

namespace EcoCentre.Models.Domain.Reporting.Journal
{
    public class JournalReportQueryParams
    {
        public bool Xls { get; set; }
        public int Page { get; set; }
        public string City { get; set; }
        public string Material { get; set; }
        public string HubId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public SortDir OrderDir { get; set; }
        public JournalReportSortBy OrderBy { get; set; }
        public int? PageSize { get; set; }
    }
}