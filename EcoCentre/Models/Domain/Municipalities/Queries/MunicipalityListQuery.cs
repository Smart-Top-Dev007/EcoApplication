using System.Collections.Generic;
using System.Linq;

namespace EcoCentre.Models.Domain.Municipalities.Queries
{
    public class MunicipalityListQuery
    {
        private readonly Repository<Municipality> _municipalityRepository;

        public MunicipalityListQuery(Repository<Municipality> municipalityRepository)
        {
            _municipalityRepository = municipalityRepository;
        }

        public IEnumerable<Municipality> Execute()
        {
            return _municipalityRepository.Query.OrderBy(x=>x.Name).ToList();
        } 
    }
}