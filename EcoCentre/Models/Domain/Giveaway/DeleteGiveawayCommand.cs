using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class DeleteGiveawayCommand
	{
		private readonly Repository<GiveawayItem> _repository;

		public DeleteGiveawayCommand(Repository<GiveawayItem> repository)
		{
			_repository = repository;
		}

		public string Id { get; set; }
		
		public void Execute()
		{
			var item = _repository.FindOne(Id);

			if (item == null || item.IsDeleted)
			{
				throw new NotFoundException();
			}

			item.IsDeleted = true;

			_repository.Save(item);
		}
	}
}