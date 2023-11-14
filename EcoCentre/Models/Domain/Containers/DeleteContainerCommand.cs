using System;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Containers
{
	public class DeleteContainerCommand
	{
		private readonly Repository<Container> _repository;

		public DeleteContainerCommand(Repository<Container> repository)
		{
			_repository = repository;
		}
		public string Id { get; set; }
		
		public void Execute()
		{
			var container = _repository.FindOne(Id);

			if (container == null || container.IsDeleted)
			{
				throw new NotFoundException();
			}

			container.IsDeleted = true;
			container.DateDeleted = DateTime.UtcNow;

			_repository.Save(container);
		}
	}
}