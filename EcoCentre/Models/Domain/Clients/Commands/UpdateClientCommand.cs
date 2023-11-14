using System;
using System.Linq;
using EcoCentre.Models.Domain.Municipalities;
using EcoCentre.Models.ViewModel;
using FluentValidation;
using EcoCentre.Models.Domain.Hubs.Queries;
using MongoDB.Driver;

namespace EcoCentre.Models.Domain.Clients.Commands
{
    using System.Collections.Generic;
    using FluentValidation.Results;
    using GlobalSettings.Queries;
    using Invoices;
    using User;

    public class UpdateClientCommand
    {
        private readonly GlobalSettingsQuery _globalSettingsQuery;
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<ClientAddress> _clientAddressRepository; 
        private readonly Repository<Municipality> _municipalityRepository;
        private readonly ClientViewModelValidator _validator;
        private readonly ClientAddressQuery _clientAddressQuery;
        private readonly HubDetailsQuery _hubDetailsQuery;
        private readonly AuthenticationContext _context;
		
        public UpdateClientCommand(GlobalSettingsQuery globalSettingsQuery, Repository<Client> clientRepository,
            Repository<ClientAddress> clientAddressRepository, Repository<Invoice> invoiceRepository, 
            Repository<Municipality> municipalityRepository, ExistingClientViewModelValidator validator, ClientAddressQuery clientAddressQuery,
            HubDetailsQuery hubDetailsQuery, AuthenticationContext context)
        {
            _context = context;
            _clientRepository = clientRepository;
            _municipalityRepository = municipalityRepository;
            _validator = validator;
            _validator.UpdateOperation = true;
            _clientAddressQuery = clientAddressQuery;
            _hubDetailsQuery = hubDetailsQuery;
            _clientAddressRepository = clientAddressRepository;
			
            _globalSettingsQuery = globalSettingsQuery;
            _invoiceRepository = invoiceRepository;
        }

        public Client Execute(ExistingClientViewModel vm, string hostUrl, string notificationsEmailAddress)
        {
            var client = _clientRepository.Collection.AsQueryable().SingleOrDefault(x => x.Id == vm.Id);
            if (client == null)
                throw new Exception("Client not found");

			if (!string.IsNullOrEmpty(vm.Address.PostalCode))
			{
				vm.Address.PostalCode = vm.Address.PostalCode.Trim().ToUpper();
				vm.Address.PostalCode = vm.Address.PostalCode.Replace("-", "");
			}

			if (vm.UpdateOnlyStatus)
            {
                client.Status = vm.Status;
                _clientRepository.Save(client);
                return client;
            }

			ClientAddress address = null;
            if (!vm.UpdateOnlyPersonalVisitsLimit)
            {
                var validationResult = _validator.Validate(vm);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);

                client.UpdateContact(vm.Email, vm.PhoneNumber, vm.MobilePhoneNumber);
                client.UpdateName(vm.FirstName, vm.LastName);
                client.Category = vm.Category;
                if(vm.Category == "OBNL")
                {
                    client.OBNLNumbers = vm.OBNLNumbers;
                }
                _hubDetailsQuery.Id = vm.Hub.Id;
                var hub = _hubDetailsQuery.Execute();
                client.Hub = hub;

                var city = _municipalityRepository.FindOne(vm.Address.CityId);

	            if (city == null)
	            {
		            city = Municipality.Create(vm.Address.NewCityName);
		            city.Enable();
		            _municipalityRepository.Insert(city);
	            }

				address = _clientAddressQuery.Execute(city, vm.Address.Street, vm.Address.CivicNumber, vm.Address.PostalCode, vm.Address.AptNumber);

                client.UpdateAddress(address);
                client.LastChange = DateTime.UtcNow;
                client.Status = vm.Status;
                client.AllowCredit = vm.AllowCredit;
	            if (vm.AllowCredit)
	            {
		            client.CreditAcountNumber = vm.CreditAcountNumber;
	            }
	            else
	            {
		            client.CreditAcountNumber = null;
	            }

                client.Comments = vm.Comments;
            }
            if (!_context.User.IsGlobalAdmin && client.PersonalVisitsLimit != null && vm.PersonalVisitsLimit != 0 && vm.PersonalVisitsLimit != client.PersonalVisitsLimit)
            {
                throw new ValidationException(new List<ValidationFailure>
                    {
                        new ValidationFailure("PersonalVisitsLimit",Resources.Model.Client.PersonalVisitsLimitChangeAccessDenied)
                    });
    
            }

            if (null == client.PersonalVisitsLimit && vm.PersonalVisitsLimit > 0 ||
                client.PersonalVisitsLimit != vm.PersonalVisitsLimit)
            {
                var globalSettings = _globalSettingsQuery.Execute();
                var thisYearDate = new DateTime(DateTime.Today.Year, 1, 1);
                var visitsCount = _invoiceRepository.Query.Count(x => x.CreatedAt > thisYearDate && x.ClientId == client.Id);

                client.VisitsLimitExceeded = (globalSettings.MaxYearlyClientVisits > 0 && vm.PersonalVisitsLimit == 0 &&
                                           visitsCount > globalSettings.MaxYearlyClientVisits) ||
                                          (vm.PersonalVisitsLimit > 0 && visitsCount > vm.PersonalVisitsLimit);
            }

            client.SetPersonalVisitsLimit(vm.PersonalVisitsLimit);
            client.Verify();
            _clientRepository.Save(client);

            if (vm.AllowAddressCreation && !vm.UpdateOnlyPersonalVisitsLimit)
            {
	            // ReSharper disable once PossibleNullReferenceException
                var street = address.Street.ToLower();
                var civicNumber = address.CivicNumber.ToUpper();
                var postalCode = address.PostalCode.ToUpper();
	            var aptNumber = address.AptNumber?.ToUpper();
	            var existingAddress = _clientAddressRepository.Query
		            .FirstOrDefault(x =>
			            x.StreetLower == street &&
			            x.CivicNumber == civicNumber &&
			            x.AptNumber == aptNumber &&
			            x.PostalCode == postalCode
		            );

	            if (existingAddress != null)
                {
                    var mailer = new CustomAddressCreatedMailer();
                    if (!string.IsNullOrEmpty(notificationsEmailAddress))
                    {
                        using (var mail = mailer.Create(client, hostUrl, notificationsEmailAddress))
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

            return client;
        }
    }
}