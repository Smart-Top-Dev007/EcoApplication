using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Queries;

namespace EcoCentre.Models.Domain.Limits.Queries
{
    public class LimitsListQuery
    {
        private readonly Repository<LimitStatus> _limitsRepository;
        private readonly Repository<Client> _clientRepository;

        public LimitsListQuery(Repository<LimitStatus> limitsRepository,Repository<Client> clientRepository )
        {
            _limitsRepository = limitsRepository;
            _clientRepository = clientRepository;
        }

        public LimitListQueryResult Execute(LimitsListQueryParams param)
        {
            var query = _limitsRepository.Query;
            if (! string.IsNullOrEmpty(param.ClientId) && param.ClientId.Length == 24)
            {
                var client = _clientRepository.FindOne(param.ClientId);
                query = query.Where(x => x.Address.Id == client.Address.Id);
            }
            var item = query.FirstOrDefault();
            if (item == null)
                item = new LimitStatus();
            return new LimitListQueryResult(item);

        }
    }
}