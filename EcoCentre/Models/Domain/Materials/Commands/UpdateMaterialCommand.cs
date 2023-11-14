using System;
using EcoCentre.Models.Domain.Materials.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.Materials.Commands
{
	public class UpdateMaterialCommand
	{
		private readonly Repository<Material> _materialRepository;
		private readonly IServiceBus _serviceBus;

		private readonly TaskRepository _taskRepository;

		public UpdateMaterialCommand(Repository<Material> materialRepository, IServiceBus serviceBus, TaskRepository taskRepository)
		{
			_materialRepository = materialRepository;
			_serviceBus = serviceBus;

			_taskRepository = taskRepository;
		}

		public void Execute(ExistingMaterialViewModel arg)
		{

			var material = _materialRepository.FindOne(arg.Id);
			var limitChanged = material.MaxYearlyAmount != arg.MaxYearlyAmount;
			var activityChanged = material.Active != arg.Active;
			material.Name = arg.Name;
			material.NameLower = material.Name.ToLower();
			material.Tag = arg.Tag;
			material.MaxYearlyAmount = arg.MaxYearlyAmount;
			material.Unit = arg.Unit;
			material.Active = arg.Active;
			material.UpdatedAt = DateTime.UtcNow;
			material.IsExcluded = arg.IsExcluded;
			material.Price = arg.Price;
			material.HubSettings = arg.HubSettings;

			_materialRepository.Save(material);

			if (limitChanged)
			{
				_serviceBus.Publish(new MaterialLimitChangedEvent
				{
					MaterialId = material.Id,
					NewLimit = arg.MaxYearlyAmount
				});
			}

			if (activityChanged)
			{
				_serviceBus.Publish(new MaterialActivityChanged
				{
					MaterialId = material.Id,
					IsActive = material.Active
				});
			}

			_taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateMaterialsBroughtTask"));
			_taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateVisitsLimitExceededTask"));
		}
	}
}