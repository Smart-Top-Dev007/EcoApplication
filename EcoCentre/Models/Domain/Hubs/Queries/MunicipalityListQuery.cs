using System.Collections.Generic;
using System.Linq;

namespace EcoCentre.Models.Domain.Hubs.Queries
{
    public class HubListQuery
    {
        private readonly Repository<Hub> _hubRepository;

        public HubListQuery(Repository<Hub> hubRepository)
        {
            _hubRepository = hubRepository;
        }

        public IEnumerable<Hub> Execute()
        {
            return _hubRepository.Query.OrderBy(x=>x.Name).ToList();
        } 
    }
}