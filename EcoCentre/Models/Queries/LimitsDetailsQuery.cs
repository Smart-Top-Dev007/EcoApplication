using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Limits;
using EcoCentre.Models.Domain.Reporting;

namespace EcoCentre.Models.Queries
{
    public class LimitsDetailsQuery
    {
        private readonly Repository<Client> _clientRepositroy;
        private readonly Repository<LimitStatus> _limitsRepository;

        public LimitsDetailsQuery(Repository<Client> clientRepositroy, Repository<LimitStatus> limitsRepository  )
        {
            _clientRepositroy = clientRepositroy;
            _limitsRepository = limitsRepository;
        }

        public LimitsDetailsQueryResult Execute(string id)
        {
            var limit = _limitsRepository.Query.SingleOrDefault(x => x.Id == id);
            var clients = _clientRepositroy.Query.Where(x => x.Address.Id == limit.Address.Id).ToList();
            return new LimitsDetailsQueryResult
                {
                    Limit = limit,
                    Clients = clients
                };

        }
    }
}