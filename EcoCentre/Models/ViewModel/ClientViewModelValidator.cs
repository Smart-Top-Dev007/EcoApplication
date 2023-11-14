using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.Municipalities;
using FluentValidation;

namespace EcoCentre.Models.ViewModel
{
	public class ClientViewModelValidator : AbstractValidator<ClientViewModel>
	{
		private readonly Repository<Client> _clientRepository;
		private readonly Repository<Municipality> _municipalityRepository;
		public bool UpdateOperation { get; set; }

		public ClientViewModelValidator(
			Repository<Client> clientRepository,
			Repository<Municipality> municipalityRepository,
			Repository<Hub> hubRepository)
		{
			_clientRepository = clientRepository;
			_municipalityRepository = municipalityRepository;
			RuleFor(x => x.FirstName)
				.NotEmpty()
				.When(x => x.Category == "résidentiel" || x.Category == "Resident")
				.WithName(Resources.Model.Client.FirstName);

			/*	RuleFor(x => x.OBNLNumber)
					.NotEmpty()
					.When(x => x.Category == "OBNL")
					.WithName(Resources.Model.Client.OBNLNumber);

				RuleFor(x => x.OBNLNumbers)
					.NotEmpty()
					.When(x => x.Category == "OBNL")
					.WithName(Resources.Model.Client.OBNLNumbers);*/

			RuleFor(x => x.LastName)
				.NotEmpty()
				.When(x => x.Category == "résidentiel" || x.Category == "Resident")
				.WithName(Resources.Model.Client.LastName);

			RuleFor(x => x.Address.PostalCode)
				.NotEmpty()
				.WithName(Resources.Model.Client.PostalCode);

			RuleFor(x => x.Address.Street)
				.NotEmpty()
				.WithName(Resources.Model.Client.Street);

			RuleFor(x => x.Address.CivicNumber)
				.NotEmpty()
				.WithName(Resources.Model.Client.CivicNumber);

			RuleFor(x => x.Email)
				.EmailAddress()
				.WithName(Resources.Model.Client.Email);


			RuleFor(x => x.Address.CityId)
				.Must(BeExistingCity)
				.WithMessage(Resources.Model.Client.InvalidCity)
				.When(x => !string.IsNullOrWhiteSpace(x.Address.CityId));

			RuleFor(x => x.Address.NewCityName)
				.NotEmpty()
				.WithMessage(Resources.Model.Client.NewCityNameMustBeSpecified)
				.When(x => string.IsNullOrEmpty(x.Address.CityId));


			RuleFor(x => x.Address.PostalCode)
				.Matches("[A-Za-z][0-9][A-Z-az][0-9][A-Za-z][0-9]")
				.WithMessage(Resources.Model.Client.InvalidPostalCodeFormat)
				.When(x => x.Address?.PostalCode != null);

			RuleFor(x => x.AllowAddressCreation)
				.Equal(true)
				.When(x=> UpdateOperation && AddressIsChanged(x))
				.WithMessage(Resources.Model.Client.AddressExistsOrCreationNotAllowed);

			RuleFor(x => x.AllowAddressCreation)
				.Equal(true)
				.When(x => !UpdateOperation && AddressExists(x))
				.WithMessage(Resources.Model.Client.AddressExistsOrCreationNotAllowed);


			RuleFor(x => x.CreditAcountNumber)
				.Matches(@"^\d{7}-\d{2}$")
				.WithMessage(Resources.Model.Client.InvalidCreditAccountFormat)
				.When(x => x.CreditAcountNumber != null);

		}

		private bool AddressExists(ClientViewModel client)
		{
			var street = client.Address.Street?.ToLower() ?? "";
			var civicNumber = client.Address.CivicNumber?.ToUpper() ?? "";
			var city = client.Address.CityId;

			var query = _clientRepository.Query.Where(x =>
				x.Address.StreetLower == street &&
				x.Address.CivicNumber == civicNumber);

			if (!string.IsNullOrWhiteSpace(city))
			{
				query = query.Where(x => x.Address.CityId == city);
			}

			var existing = query.FirstOrDefault();

			return existing != null;
		}

		private bool AddressIsChanged(ClientViewModel client)
		{
			var street = client.Address.Street?.ToLower() ?? "";
			var civicNumber = client.Address.CivicNumber?.ToUpper() ?? "";
			var city = client.Address.CityId ?? "";
			var id = client.Id;
			var existingClient = _clientRepository.Query.Single(x => x.Id == id);

			var existingAddress = existingClient.Address;

			return existingAddress.StreetLower != street ||
			       existingAddress.CivicNumber != civicNumber ||
			       existingAddress.CityId != city;
		}


		private bool BeExistingCity(string arg2)
		{
			return _municipalityRepository.Query.Any(x => x.Id == arg2);
		}
		
	}
}