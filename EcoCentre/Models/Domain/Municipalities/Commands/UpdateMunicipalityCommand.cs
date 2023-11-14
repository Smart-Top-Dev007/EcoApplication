using EcoCentre.Models.Domain.Municipalities.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.Municipalities.Commands
{
    public class UpdateMunicipalityCommand
    {
        private readonly Repository<Municipality> _municipalityRepository;
        private readonly IServiceBus _serviceBus;

        public UpdateMunicipalityCommand(Repository<Municipality> municipalityRepository, IServiceBus serviceBus)
        {
            _municipalityRepository = municipalityRepository;
            _serviceBus = serviceBus;
        }

        public void Execute(string id, string name, bool enabled, string hubId)
        {

            var municipality = _municipalityRepository.FindOne(id);
            municipality.UpdateName(name);
            if(enabled)
                municipality.Enable();
            else
                municipality.Disable();

	        municipality.HubId = hubId;

			_municipalityRepository.Save(municipality);
            _serviceBus.Publish(new MunicipalityUpdatedEvent(municipality));
        }
    }
}