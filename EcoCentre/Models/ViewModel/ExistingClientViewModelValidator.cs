using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.Municipalities;

namespace EcoCentre.Models.ViewModel
{
	public class ExistingClientViewModelValidator : ClientViewModelValidator
	{
		public ExistingClientViewModelValidator(Repository<Client> clientRepository, Repository<Municipality> municipalityRepository, Repository<Hub> hubRepository) : 
			base(clientRepository, municipalityRepository, hubRepository)
		{

		}

	}
}