namespace EcoCentre.Models.Domain.Municipalities.Queries
{
    public class MunicipalityDetailsQuery
    {
        private readonly Repository<Municipality> _municipalityRepository;

        public MunicipalityDetailsQuery(Repository<Municipality> municipalityRepository)
        {
            _municipalityRepository = municipalityRepository;
        }

        public Municipality Execute(string id)
        {
            return _municipalityRepository.FindOne(id);
        } 
    }
}