using System;
using EcoCentre.Models.Domain.Common;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class OBNLReinvestmentListQueryParams
    {
        public string Id { get; set; }
        public bool Deleted { get; set; }
        public string UserId { get; set; }
        public int Page { get; set; }
        public string Term { get; set; }
        public bool CurrentYear { get; set; }
        public SortDir SortDir { get; set; }
        public OBNLReinvestmentSortTerm SortBy { get; set; }
        public OBNLReinvestmentSearchBy Type { get; set; }
        public DateTime? TermFrom { get; set; }
        public DateTime? TermTo { get; set; }
        public string CenterName { get; set; }

        public int? PageSize { get; set; }
    }
}