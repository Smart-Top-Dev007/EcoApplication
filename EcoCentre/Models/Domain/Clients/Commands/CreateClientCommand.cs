using System;
using System.Linq;
using EcoCentre.Models.Domain.Municipalities;
using EcoCentre.Models.ViewModel;
using FluentValidation;
using EcoCentre.Models.Domain.Hubs.Queries;
using EcoCentre.Models.Domain.User;

namespace EcoCentre.Models.Domain.Clients.Commands
{
    public class CreateClientCommand
    {
        private readonly Repository<Client> _clientRepository;
		private readonly Repository<Client1> _client1Repository;
		private readonly Repository<ClientAddress> _clientAddressRepository;
		private readonly Repository<ClientAddress1> _clientAddressRepository1;
		private readonly Repository<Municipality> _municipalityRepository;
        private readonly ClientViewModelValidator _validator;
		private readonly ClientAddressQuery _clientAddressQuery, _clientAddressQuery1;
        private readonly HubDetailsQuery _hubDetailsQuery;
		private readonly AuthenticationContext _authContext;

		public CreateClientCommand(Repository<Client> clientRepository, Repository<Client1> client1Repository, Repository<ClientAddress> clientAddressRepository, Repository<ClientAddress1> clientAddressRepository1, Repository<Municipality> municipalityRepository, ClientViewModelValidator validator, ClientAddressQuery clientAddressQuery, ClientAddressQuery clientAddressQuery1, HubDetailsQuery hubDetailsQuery, AuthenticationContext authContext)
        {
			_authContext = authContext;
			_clientRepository = clientRepository;
			_client1Repository = client1Repository;
			_municipalityRepository = municipalityRepository;
            _validator = validator;
            _clientAddressQuery = clientAddressQuery;
			_clientAddressQuery1 = clientAddressQuery1;
			_hubDetailsQuery = hubDetailsQuery;
            _clientAddressRepository = clientAddressRepository;
			_clientAddressRepository1 = clientAddressRepository1;
		}

        public Client Execute(ClientViewModel vm, string hostUrl, string notificationsEmailAddress)
        {
            if(!string.IsNullOrEmpty(vm.Address.PostalCode))
            {
                vm.Address.PostalCode = vm.Address.PostalCode.Trim().ToUpper();
                vm.Address.PostalCode = vm.Address.PostalCode.Replace("-", "");
            }
            _validator.ValidateAndThrow(vm);
            var client = new Client();
	        var city = _municipalityRepository.FindOne(vm.Address.CityId);

	        if (city == null)
	        {
		        city = Municipality.Create(vm.Address.NewCityName);
				city.Enable();
				_municipalityRepository.Insert(city);
			}

            _hubDetailsQuery.Id = vm.Hub.Id;
            var hub = _hubDetailsQuery.Execute();
            client.Hub = hub;

            client.RegistrationDate = DateTime.UtcNow;
            client.UpdateName(vm.FirstName, vm.LastName);
            client.UpdateContact(vm.Email, vm.PhoneNumber, vm.MobilePhoneNumber);
            //
            client.OBNLNumber = vm.OBNLNumber;
            client.OBNLNumbers = vm.OBNLNumbers;
            client.Category = vm.Category.ToLower();
            client.CategoryCustom = vm.CategoryCustom;

            var address = _clientAddressQuery.Execute(city, vm.Address.Street, vm.Address.CivicNumber,
                                                      vm.Address.PostalCode, vm.Address.AptNumber);
            client.UpdateAddress(address);
            client.LastChange = DateTime.UtcNow;
            client.SetPersonalVisitsLimit(vm.PersonalVisitsLimit);
            client.Comments = vm.Comments;
            client.AllowCredit = vm.AllowCredit;
			

			if (vm.AllowCredit)
	        {
		        client.CreditAcountNumber = vm.CreditAcountNumber;
			}
	        else
	        {
		        client.CreditAcountNumber = null;
	        }

	        client.Verify();
            _clientRepository.Save(client);

	        try
	        {
		        if (vm.AllowAddressCreation)
		        {

			        var street = address.Street.ToLower();
			        var civicNumber = address.CivicNumber.ToUpper();
			        var postalCode = address.PostalCode.ToUpper();
			        var aptNumber = address.AptNumber?.ToUpper();
			        var existingAddress = _clientAddressRepository.Query
				        .FirstOrDefault(x =>
					        x.StreetLower == street
					        && x.CivicNumber == civicNumber
					        && x.AptNumber == aptNumber
					        && x.PostalCode == postalCode
				        );

			        if (existingAddress != null && !(_authContext.User.IsGlobalAdmin || _authContext.User.IsAdmin))
					{

				        if (!string.IsNullOrEmpty(notificationsEmailAddress))
				        {
					        var mailer = new CustomAddressCreatedMailer();
					        using (var mail = mailer.Create(client, hostUrl, notificationsEmailAddress))
					        {
						        try
						        {
							        mail.Send();
						        }
						        catch
						        {
							        // do nothing, it's a stub so that saving code would go through
						        }
					        }
				        }
			        }
		        }
	        }
	        catch (Exception ex)
	        {
				throw new Exception($"Failed to send email about new address creation. Client Id: {client.Id}", ex);
	        }

	        return client;

        }

		public Client1 Execute1(ClientViewModel vm, string hostUrl, string notificationsEmailAddress)
		{
			if (!string.IsNullOrEmpty(vm.Address.PostalCode))
			{
				vm.Address.PostalCode = vm.Address.PostalCode.Trim().ToUpper();
				vm.Address.PostalCode = vm.Address.PostalCode.Replace("-", "");
			}
			_validator.ValidateAndThrow(vm);
			var client = new Client1();
			var city = _municipalityRepository.FindOne(vm.Address.CityId);

			if (city == null)
			{
				city = Municipality.Create(vm.Address.NewCityName);
				city.Enable();
				_municipalityRepository.Insert(city);
			}

			_hubDetailsQuery.Id = vm.Hub.Id;
			var hub = _hubDetailsQuery.Execute();
			client.Hub = hub;

			client.RegistrationDate = DateTime.UtcNow;
			client.UpdateName(vm.FirstName, vm.LastName);
			client.UpdateContact(vm.Email, vm.PhoneNumber, vm.MobilePhoneNumber);
			//
			client.OBNLNumber = vm.OBNLNumber;
			client.OBNLNumbers = vm.OBNLNumbers;
			client.Category = vm.Category.ToLower();
			client.CategoryCustom = vm.CategoryCustom;

			var address = _clientAddressQuery1.Execute1(city, vm.Address.Street, vm.Address.CivicNumber,
													  vm.Address.PostalCode, vm.Address.AptNumber);
			client.UpdateAddress(address);
			client.LastChange = DateTime.UtcNow;
			client.SetPersonalVisitsLimit(vm.PersonalVisitsLimit);
			client.Comments = vm.Comments;
			client.AllowCredit = vm.AllowCredit;


			if (vm.AllowCredit)
			{
				client.CreditAcountNumber = vm.CreditAcountNumber;
			}
			else
			{
				client.CreditAcountNumber = null;
			}

			client.Verify();
			_client1Repository.Save(client);

			try
			{
				if (vm.AllowAddressCreation)
				{

					var street = address.Street.ToLower();
					var civicNumber = address.CivicNumber.ToUpper();
					var postalCode = address.PostalCode.ToUpper();
					var aptNumber = address.AptNumber?.ToUpper();
					var existingAddress = _clientAddressRepository1.Query
						.FirstOrDefault(x =>
							x.StreetLower == street
							&& x.CivicNumber == civicNumber
							&& x.AptNumber == aptNumber
							&& x.PostalCode == postalCode
						);

					if (existingAddress != null && !(_authContext.User.IsGlobalAdmin || _authContext.User.IsAdmin))
					{

						if (!string.IsNullOrEmpty(notificationsEmailAddress))
						{
							var mailer = new CustomAddressCreatedMailer();
							using (var mail = mailer.Create1(client, hostUrl, notificationsEmailAddress))
							{
								try
								{
									mail.Send();
								}
								catch
								{
									// do nothing, it's a stub so that saving code would go through
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Failed to send email about new address creation. Client Id: {client.Id}", ex);
			}

			return client;

		}

	}
}