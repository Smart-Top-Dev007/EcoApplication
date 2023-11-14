using System.Linq;
using EcoCentre.Models.Domain.Municipalities.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.Clients.Consumers
{
    public class UpdateAddressesOnMunicipalityUpdated : Consumes<MunicipalityUpdatedEvent>.All
    {
        private readonly Repository<ClientAddress> _addressRepository;
        private readonly Repository<Client> _clientRepository;

        public UpdateAddressesOnMunicipalityUpdated(Repository<ClientAddress> addressRepository, Repository<Client> clientRepository  )
        {
            _addressRepository = addressRepository;
            _clientRepository = clientRepository;
        }

        public void Consume(MunicipalityUpdatedEvent message)
        {
            var addresses = _addressRepository.Query.Where(x => x.CityId == message.MunicipalityId).ToList();
            foreach (var address in addresses)
            {
                address.City = message.Name;
                _addressRepository.Save(address);
            }

            var clients = _clientRepository.Query.Where(x => x.Address.CityId == message.MunicipalityId).ToList();
            foreach (var client in clients)
            {
                client.Address.City = message.Name;
                _clientRepository.Save(client);
            }
        }
    }

}