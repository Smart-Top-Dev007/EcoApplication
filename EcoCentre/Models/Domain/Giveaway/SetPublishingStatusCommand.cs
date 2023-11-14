using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class SetPublishingStatusCommand
	{
		private readonly Repository<GiveawayItem> _repository;

		public SetPublishingStatusCommand(Repository<GiveawayItem> repository)
		{
			_repository = repository;
		}

		public string Id { get; set; }
		public bool IsPublished { get; set; }


		public void Execute()
		{
			var item = _repository.FindOne(Id);

			if (item == null || item.IsDeleted)
			{
				throw new NotFoundException();
			}

			if (item.IsGivenAway)
			{
				throw new Exception("Can't change publishing status for items that was given away.");
			}

			item.IsPublished = IsPublished;

			_repository.Save(item);
		}
	}
}