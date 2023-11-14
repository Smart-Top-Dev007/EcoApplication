namespace EcoCentre.Models.Domain.Giveaway.Type
{
	public class AddGiveawayTypeCommand
	{
		private readonly Repository<GiveawayType> _repository;

		public AddGiveawayTypeCommand(Repository<GiveawayType> repository)
		{
			_repository = repository;
		}
		public string Name { get; set; }
		public void Execute()
		{
			var item = new GiveawayType
			{
				Name = Name
			};
			_repository.Save(item);
		}
	}
}