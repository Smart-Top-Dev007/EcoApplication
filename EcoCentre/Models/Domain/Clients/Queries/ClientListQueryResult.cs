using EcoCentre.Models.ViewModel;
using System.Collections.Generic;

namespace EcoCentre.Models.Domain.Clients.Queries
{
    public class ClientListQueryResult
    {
        public IEnumerable<ClientViewModel> Clients { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
    }

}