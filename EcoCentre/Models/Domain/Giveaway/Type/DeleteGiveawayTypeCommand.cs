using System.Linq;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Giveaway.Type
{
	public class DeleteGiveawayTypeCommand
	{
		private readonly Repository<GiveawayType> _repository;

		public DeleteGiveawayTypeCommand(Repository<GiveawayType> repository)
		{
			_repository = repository;
		}
		public string Name { get; set; }
		public void Execute()
		{
			var item = _repository.Query.FirstOrDefault(x => x.Name == Name);
			if (item == null)
			{
				throw new NotFoundException(typeof(GiveawayType), Name);
			}

			_repository.Remove(item);
		}
	}
}