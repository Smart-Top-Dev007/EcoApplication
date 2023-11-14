using System;
using System.Linq;
using EcoCentre.Models.Domain.Municipalities;
using EcoCentre.Models.ViewModel;
using EcoCentre.Models.Domain.Hubs.Queries;

namespace EcoCentre.Models.Domain.Clients.Commands
{
    public class ImportClientCommand
    {
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Municipality> _municipalityRepository;
        private readonly ClientAddressQuery _clientAddressQuery;
        private readonly HubDetailsQuery _hubDetailsQuery;

        public ImportClientCommand(Repository<Client> clientRepository, Repository<Municipality> municipalityRepository, ClientAddressQuery clientAddressQuery, HubDetailsQuery hubDetailsQuery)
        {
            _clientRepository = clientRepository;
            _municipalityRepository = municipalityRepository;
            _clientAddressQuery = clientAddressQuery;
            _hubDetailsQuery = hubDetailsQuery;
        }

		public Client Execute(ClientImportModel vm, bool extended = false)
		{
			if (vm.Address != null && !string.IsNullOrEmpty(vm.Address.PostalCode))
			{
				vm.Address.PostalCode = vm.Address.PostalCode.Trim().ToUpper();
				vm.Address.PostalCode = vm.Address.PostalCode.Replace(" ", "-");
			}
			Client client = null;
			if (!string.IsNullOrEmpty(vm.RefId))
			{
				client = _clientRepository.Query.SingleOrDefault(x => x.RefId == vm.RefId);
			}
			if (extended && client == null)
			{
				client = _clientRepository.Query.SingleOrDefault(c => c.CitizenCard.EndsWith(vm.CitizenCard));
			}

			if (client == null)
			{
				client = new Client();
				client.RegistrationDate = DateTime.UtcNow;
				client.RefId = vm.RefId;
			}
			
			client.UpdateName(vm.FirstName, vm.LastName);
			client.UpdateContact(vm.Email, vm.PhoneNumber, null);

			client.Category = vm.Category.ToLower();
			Municipality city;
			if (extended)
			{
				city = _municipalityRepository.Query.FirstOrDefault(x => x.NameLower == vm.Address.City.ToLower());
			}
			else
			{
				city = _municipalityRepository.Query.SingleOrDefault(x => x.NameLower == vm.Address.City.ToLower());
			}
            if (city == null)
            {
                city = Municipality.Create(vm.Address.City);
                _municipalityRepository.Save(city);
            }

            client.MunicipalityId = city.Id;

            _hubDetailsQuery.Id = vm.HubId;
            var hub = _hubDetailsQuery.Execute();
            client.Hub = (Hubs.Hub)hub;

            var address = _clientAddressQuery.Execute(city, vm.Address.Street, vm.Address.CivicNumber,
                                                      vm.Address.PostalCode, vm.Address.AptNumber);
            client.UpdateAddress(address);
            client.LastChange = DateTime.UtcNow;
            client.Comments = vm.Comments;

            if (extended)
            {
	            client.CitizenCard = vm.CitizenCard;
	            client.Gender = vm.Gender;
	            client.DateFinMember = vm.DateFinMember;
	            client.UpdateCitizenCard();
			}

            _clientRepository.Save(client);
            return client;

        }
    }
}