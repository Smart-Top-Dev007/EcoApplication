using System.Collections.Generic;
using System.Linq;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class GiveawayItemQuery
	{
		private readonly Repository<GiveawayItem> _repository;

		public GiveawayItemQuery(Repository<GiveawayItem> repository)
		{
			_repository = repository;
		}

		public string Id { get; set; }

		public GiveawayItem Execute()
		{
			return _repository.Query.FirstOrDefault(x=> !x.IsDeleted &&  x.Id == Id);
		}
	}
}