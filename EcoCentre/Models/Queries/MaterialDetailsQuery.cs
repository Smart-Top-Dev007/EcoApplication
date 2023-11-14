using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Queries
{
    public class MaterialDetailsQuery
    {
        private readonly Repository<Material> _materialRepository;

        public MaterialDetailsQuery(Repository<Material> materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public Material Execute(string id)
        {
            return _materialRepository.Query.FirstOrDefault(x => x.Id == id);
        } 
    }
}