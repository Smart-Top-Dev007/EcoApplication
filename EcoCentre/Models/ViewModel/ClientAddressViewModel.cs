using EcoCentre.Models.Domain.Clients;
namespace EcoCentre.Models.ViewModel
{
	public class ClientAddressViewModel
	{

		public string City { get; set; }
		public string CityId { get; set; }
		public string PostalCode { get; set; }
		public string Street { get; set; }
		public string CivicNumber { get; set; }
		public string NewCityName { get; set; }
		public int ExternalId { get; set; }
		public string AptNumber { get; set; }


		public ClientAddressViewModel()
		{
		}

		public ClientAddressViewModel(ClientAddress address)
		{
			if (address != null)
			{
				City = address.City;
				CityId = address.CityId;
				PostalCode = address.PostalCode;
				Street = address.Street;
				CivicNumber = address.CivicNumber;
				ExternalId = address.ExternalId;
				AptNumber = address.AptNumber;
			}
		}
	}
}