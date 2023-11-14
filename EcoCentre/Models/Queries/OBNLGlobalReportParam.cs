using System;
using EcoCentre.Models.Domain.Clients.Queries;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.Invoices;

namespace EcoCentre.Models.Queries
{
    public class OBNLGlobalReportParam
    {
        public OBNLTotalReportOrderBy SortBy { get; set; }
        public SortDir SortDir { get; set; }
        public string CenterName { get; set; }
        public string OBNLNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public bool Xls { get; set; }
    }
}