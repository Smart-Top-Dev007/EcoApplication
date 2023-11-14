using System.Linq;

namespace EcoCentre.Models.Domain.Clients.Queries
{
	public class SuggestClientNameQuery1
	{
		private readonly Repository<Client1> _clientRepository1;

		public SuggestClientNameQuery1(Repository<Client1> clientRepository1)
		{
			_clientRepository1 = clientRepository1;
		}

		public string[] Execute(string term, bool isFirstName)
		{
			if(string.IsNullOrEmpty(term)) return new string[]{};
			term = term.ToLower();
			if (!isFirstName)
			{
				return _clientRepository1.Query.Where(x => x.LastName.ToLower().StartsWith(term))
										.OrderBy(x => x.LastName)
										.Take(30)
										.Select(x => x.LastName)
										.ToList()
										.Distinct().ToArray();
			}
			else
			{
				return _clientRepository1.Query.Where(x => x.FirstName.ToLower().StartsWith(term))
										.OrderBy(x => x.FirstName)
										.Take(30)
										.Select(x => x.FirstName)
										.ToList()
										.Distinct().ToArray();
			}
		}
	}
}
