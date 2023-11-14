using System.Linq;
using EcoCentre.Models.Domain.Municipalities.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.Limits.Consumers
{
    public class UpdateLimitsAddressOnMunicipalityChanged : Consumes<MunicipalityUpdatedEvent>.All
    {
        private readonly Repository<LimitStatus> _limitsRepository;

        public UpdateLimitsAddressOnMunicipalityChanged(Repository<LimitStatus> limitsRepository)
        {
            _limitsRepository = limitsRepository;
        }

        public void Consume(MunicipalityUpdatedEvent message)
        {
            var limits = _limitsRepository.Query.Where(x => x.Address.CityId == message.MunicipalityId).ToList();
            foreach (var limit in limits)
            {
                limit.Address.City = message.Name;
                _limitsRepository.Save(limit);
            }
        }
    }
}