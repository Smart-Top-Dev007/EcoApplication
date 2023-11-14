using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.User;
using FluentValidation;

namespace EcoCentre.Models.Domain.Containers
{
	public class CreateContainerCommand
	{
		private readonly CreateContainerCommandValidator _validator;
		private readonly Repository<Container> _repository;
		private readonly Repository<Material> _materialRepository;
		private readonly AuthenticationContext _context;


		public CreateContainerCommand(CreateContainerCommandValidator validator, Repository<Container> repository, AuthenticationContext context, Repository<Material> materialRepository)
		{
			_validator = validator;
			_repository = repository;
			_context = context;
			_materialRepository = materialRepository;
		}
		public string Number { get; set; }
		public List<string> MaterialIds{ get; set; }
		public decimal Capacity { get; set; }
		public decimal AlertAtAmount { get; set; }
		
		public void Execute()
		{
			_validator.ValidateAndThrow(this);


			//var materials = _materialRepository.GetMany(MaterialIds);
			//foreach (var material in materials)
			//{
			//	var allowedInContainer = material.IsAllowedToPutToContainer(_context.Hub.Id);
			//	if (!allowedInContainer)
			//	{
			//		throw new ValidationException($"Matériau '{material.Name}' ne peut pas être placé dans ce conteneur");
			//	}
			//}
			
			var containerMaterials = MaterialIds
				.Select(x => new ContainerMaterial { Id = x })
				.ToList();

			var item = new Container
			{
				DateAdded = DateTime.UtcNow,
				DateOfLastAlert = null,
				AlertAtAmount = AlertAtAmount,
				Capacity = Capacity,
				FillAmount = 0,
				Number = Number,
				Materials = containerMaterials,
				HubId = _context.Hub.Id
			};

			_repository.Save(item);
		}
	}
}