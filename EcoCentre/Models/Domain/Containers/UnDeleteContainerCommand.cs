using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Containers
{
	public class UndeleteContainerCommand
	{
		private readonly Repository<Container> _repository;

		public UndeleteContainerCommand(Repository<Container> repository)
		{
			_repository = repository;
		}
		public string Id { get; set; }
		
		public void Execute()
		{
			var container = _repository.FindOne(Id);

			if (container == null)
			{
				throw new NotFoundException();
			}

			container.IsDeleted = false;

			_repository.Save(container);
		}
	}
}