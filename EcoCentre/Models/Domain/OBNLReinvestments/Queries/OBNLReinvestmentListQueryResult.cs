using System.Collections.Generic;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class OBNLReinvestmentListQueryResult
    {
        public IEnumerable<OBNLReinvestmentDetails> OBNLReinvestments { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
    }
}