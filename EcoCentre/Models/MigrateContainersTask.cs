using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Containers;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models
{
	public class MigrateContainersTask:AsyncAdminTask
	{
		private readonly Repository<Container> _repository;

		public MigrateContainersTask(Repository<Container> repository)
		{
			_repository = repository;
		}

		protected override void DoWork()
		{
			var containers = _repository.Query.ToList();

			foreach (var container in containers)
			{
				if (container.Materials == null)
				{
					container.Materials = new List<ContainerMaterial>();
				}

				if (container.MaterialId != null && container.Materials.All(x => x.Id != container.MaterialId))
				{
					container.Materials.Add(new ContainerMaterial{Id = container.MaterialId});
					container.MaterialId = null;
				}
				_repository.Save(container);
			}
		}
	}
}