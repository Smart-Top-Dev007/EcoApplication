using System.Linq;
using EcoCentre.Models.Domain.Municipalities;

namespace EcoCentre.Models.Domain.Clients.Commands
{
	public class ClientAddressQuery
	{
		private readonly Repository<ClientAddress> _addressRepository;
		private readonly Repository<ClientAddress1> _addressRepository1;

		public ClientAddressQuery(Repository<ClientAddress> addressRepository, Repository<ClientAddress1> addressRepository1)
		{
			_addressRepository = addressRepository;
			_addressRepository1 = addressRepository1;
		}

		public ClientAddress Execute(Municipality city, string street, string civicNumber,
			string postalCode, string aptNumber)
		{
            postalCode = postalCode?.Trim().ToUpper() ?? string.Empty;

			aptNumber = aptNumber?.Trim();

			var address = _addressRepository.Query
				.FirstOrDefault(x =>
					x.CityId == city.Id &&
					x.CivicNumber == civicNumber.ToUpper().Trim() &&
					x.StreetLower == street.Trim().ToLower() &&
					x.AptNumber == aptNumber &&
					x.PostalCode == postalCode
				);

			if (address == null)
			{
				address = new ClientAddress();
				address.UpdateDetails(city,street,civicNumber,postalCode, aptNumber);
				_addressRepository.Insert(address);
			}
			return address;

		}

		public ClientAddress1 Execute1(Municipality city, string street, string civicNumber,
			string postalCode, string aptNumber)
		{
			postalCode = postalCode?.Trim().ToUpper() ?? string.Empty;

			aptNumber = aptNumber?.Trim();

			var address = _addressRepository1.Query
				.FirstOrDefault(x =>
					x.CityId == city.Id &&
					x.CivicNumber == civicNumber.ToUpper().Trim() &&
					x.StreetLower == street.Trim().ToLower() &&
					x.AptNumber == aptNumber &&
					x.PostalCode == postalCode
				);

			if (address == null)
			{
				address = new ClientAddress1();
				address.UpdateDetails(city, street, civicNumber, postalCode, aptNumber);
				_addressRepository1.Insert(address);
			}
			return address;

		}
	}
}