using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;
using FluentValidation;

namespace EcoCentre.Models.Domain.Containers
{
	public class UpdateContainerCommand
	{
		private readonly UpdateContainerCommandValidator _validator;
		private readonly Repository<Container> _repository;
		private readonly Repository<Material> _materialRepository;
		private readonly AuthenticationContext _context;

		public UpdateContainerCommand(UpdateContainerCommandValidator validator, Repository<Container> repository, Repository<Material> materialRepository, AuthenticationContext context)
		{
			_validator = validator;
			_repository = repository;
			_materialRepository = materialRepository;
			_context = context;
		}

		public string Id { get; set; }
		public string Number { get; set; }
		public List<string> MaterialIds { get; set; }
		public decimal Capacity { get; set; }
		public decimal AlertAtAmount { get; set; }

		public void Execute()
		{
			_validator.ValidateAndThrow(this);

			var container = _repository.FindOne(Id);

			if (container == null || container.IsDeleted)
			{
				throw new NotFoundException();
			}

			var materials = _materialRepository.GetMany(MaterialIds);
			foreach (var material in materials)
			{
				var allowedInContainer = material.IsAllowedToPutToContainer(_context.Hub.Id);
				if (!allowedInContainer)
				{
					throw new NotFoundException($"Matériau '{material.Name}' ne peut pas être placé dans ce conteneur");
					throw new ValidationException($"Matériau '{material.Name}' ne peut pas être placé dans ce conteneur");
					
				}
			}

			container.AlertAtAmount = AlertAtAmount;
			container.Capacity = Capacity;
			container.Number = Number;

			container.Materials = materials
				.Select(x => new ContainerMaterial{Id = x.Id})
				.ToList();

			_repository.Save(container);

		}
	}
}