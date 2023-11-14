using System.Collections.Generic;
using System.Linq;

namespace EcoCentre.Models.Domain.Giveaway.Type
{
	public class GiveawayTypeQuery
	{
		private readonly Repository<GiveawayType> _repository;

		public GiveawayTypeQuery(Repository<GiveawayType> repository)
		{
			_repository = repository;
		}
		public List<GiveawayType> Execute()
		{
			return _repository.Query.ToList();
		}
	}
}