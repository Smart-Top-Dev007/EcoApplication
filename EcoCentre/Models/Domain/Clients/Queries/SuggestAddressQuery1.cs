using System.Linq;

namespace EcoCentre.Models.Domain.Clients.Queries
{
	using System;

	public class SuggestAddressQuery1
	{
		private readonly Repository<ClientAddress1> _clientAddressRepository;

		public SuggestAddressQuery1(Repository<ClientAddress1> clientAddressRepository)
		{
			_clientAddressRepository = clientAddressRepository;
		}

		public string[] Execute(string number, string streetName, string postalCode, string type, string cityId = null, string hubId = null)
		{
			if (string.IsNullOrWhiteSpace(streetName) && string.IsNullOrWhiteSpace(number) && string.IsNullOrWhiteSpace(postalCode))
			{
				return Array.Empty<string>();
			}

			streetName = streetName?.Trim() ?? "";
			number = number?.Trim() ?? "";
			postalCode = postalCode?.Trim() ?? "";

			var query = _clientAddressRepository.Query;

			if (!string.IsNullOrWhiteSpace(streetName))
			{
				query = query.Where(x => x.Street.ToLower().Contains(streetName));
			}

			if (type == "number" || type == "postalCode")
			{
				query = query.Where(x => x.CivicNumber.StartsWith(number, StringComparison.OrdinalIgnoreCase));
				query = query.Where(x => x.PostalCode.StartsWith(postalCode, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(cityId))
			{
				query = query.Where(x => x.CityId == cityId);
			}

			var searchResult = query
				.OrderBy(x => x.Street)
				.Take(20)
				.ToList();

			switch (type)
			{
				case "number":
					return searchResult.Select(x => x.CivicNumber).ToArray();
				case "postalCode":
					return searchResult.Select(x => x.PostalCode).ToArray();
				case "streetName":
					return searchResult.Select(x => x.Street).ToArray();
				default:
					return searchResult.Select(x => $"{x.PostalCode}, {x.CivicNumber}, {x.Street}").ToArray();
			}
		}
	}
}