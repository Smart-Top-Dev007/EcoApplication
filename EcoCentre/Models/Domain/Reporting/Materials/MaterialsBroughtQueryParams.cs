using System;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Queries;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    public class MaterialsBroughtQueryParams
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool Xls { get; set; }
        public SortDir SortDir { get; set; }
        public MaterialsBroughtBySortBy SortBy { get; set; }
	    public string HubId { get; set; }
	    public string MunicipalityId { get; set; }
    }
}