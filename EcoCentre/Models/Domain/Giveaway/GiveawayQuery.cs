using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.User;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class GiveawayQuery
	{
		private readonly Repository<GiveawayItem> _repository;
		private readonly AuthenticationContext _context;

		public GiveawayQuery(Repository<GiveawayItem> repository, AuthenticationContext context)
		{
			_repository = repository;
			_context = context;
		}

		public string Q { get; set; }
		public bool OnlyCurrentHub { get; set; }
		public bool? IsGivenAway { get; set; }

		public List<GiveawayItem> Execute()
		{
			var repositoryQuery = _repository.Query.Where(x=> !x.IsDeleted);

			if (!string.IsNullOrWhiteSpace(Q))
			{
				repositoryQuery = repositoryQuery.Where(x =>
					x.TitleLower.Contains(Q) ||
					x.DescriptionLower.Contains(Q) ||
					x.TypeLower.Contains(Q));
			}

			if (OnlyCurrentHub)
			{
				var hubId = _context.Hub?.Id;
				repositoryQuery = repositoryQuery.Where(x => x.HubId == hubId);
			}

			if (IsGivenAway != null)
			{
				repositoryQuery = repositoryQuery.Where(x => x.IsGivenAway == IsGivenAway);
			}

			return repositoryQuery.OrderByDescending(x=>x.DateAdded).ToList();
		}
	}
}