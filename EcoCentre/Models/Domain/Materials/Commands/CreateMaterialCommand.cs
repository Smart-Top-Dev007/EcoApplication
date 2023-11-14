using System;

namespace EcoCentre.Models.Domain.Materials.Commands
{
    public class CreateMaterialCommand
    {
        private readonly Repository<Material> _materialRepository;

        public CreateMaterialCommand(Repository<Material> materialRepository)
        {
            _materialRepository = materialRepository;
        }

	    public void Execute(MaterialViewModel arg)
	    {
		    var material = new Material
		    {
			    Name = arg.Name,
			    NameLower = arg.Name?.ToLower(),
			    Price = arg.Price,
			    Tag = arg.Tag,
			    Unit = arg.Unit,
			    Weight = arg.Weight,
			    MaxYearlyAmount = arg.MaxYearlyAmount,
			    CreatedAt = DateTime.UtcNow,
			    UpdatedAt = DateTime.UtcNow,
			    Active = true,
			    IsExcluded = arg.IsExcluded,
			    HubSettings = arg.HubSettings
		    };
		    _materialRepository.Save(material);
	    }
    }
}