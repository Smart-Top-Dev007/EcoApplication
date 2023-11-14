using System.Linq;
using EcoCentre.Models.Domain.Clients.Commands;
using EcoCentre.Models.Domain.Municipalities;
using EcoCentre.Models.Domain.Reporting.Journal.Tasks;

namespace EcoCentre.Models.Domain.Clients.Tasks
{
    public class UpdateAddressesAndClients : AsyncAdminTask
    {
        private readonly Repository<Client> _clientRepository;
        private readonly ClientAddressQuery _clientAddressQuery;
        private readonly Repository<Municipality> _municipalityRepository;

        public UpdateAddressesAndClients(Repository<Client> clientRepository, 
            ClientAddressQuery clientAddressQuery, Repository<Municipality> municipalityRepository )
        {
            _clientRepository = clientRepository;
            _clientAddressQuery = clientAddressQuery;
            _municipalityRepository = municipalityRepository;
        }

        protected override void DoWork()
        {
            var clients = _clientRepository.Query.ToList();

            var cities = _municipalityRepository.Query.ToList();
            foreach (var client in clients)
            {
                if (client.Address.Id == null)
                {
                    var city = cities.First(x => x.Id == client.Address.CityId);
                    var address = _clientAddressQuery.Execute(city, client.Address.StreetLower,
                                                              client.Address.CivicNumber, client.Address.PostalCode, client.Address.AptNumber);
                    client.Address.Id = address.Id;
                    _clientRepository.Save(client);
                }
            }
        }
    }
}