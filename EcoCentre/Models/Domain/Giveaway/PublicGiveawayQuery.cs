using System;
using System.Linq;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.ViewModel.Giveaway;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class PublicGiveawayQuery
	{
		private readonly Repository<GiveawayItem> _repository;
		private readonly Repository<Hub> _hubRepository;

		public PublicGiveawayQuery(Repository<GiveawayItem> repository, Repository<Hub> hubRepository)
		{
			_repository = repository;
			_hubRepository = hubRepository;
		}

		public string HubName { get; set; }

		public PublicListingViewModel Execute()
		{
			Hub hub = null;
			var hubs = _hubRepository.Query.ToList();

			var repositoryQuery = _repository.Query.Where(x =>
				!x.IsDeleted
				&& x.IsPublished
				&& x.IsGivenAway == false );
			
			if (!string.IsNullOrWhiteSpace(HubName))
			{
				hub = hubs.FirstOrDefault(x => x.Name.Equals(HubName, StringComparison.CurrentCultureIgnoreCase));
				if (hub != null)
				{
					repositoryQuery = repositoryQuery.Where(x => x.HubId == hub.Id);
				}
			}

			var items = repositoryQuery.OrderByDescending(x=>x.DateAdded).ToList();

			return new PublicListingViewModel
			{
				Items = items,
				SelectedHub = hub,
				Hubs = hubs
			};
		}
	}
}