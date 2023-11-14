namespace EcoCentre.Models.Domain.Municipalities.Commands
{
    public class CreateMunicipalityCommand
    {
        private readonly Repository<Municipality> _municipalityRepository;

        public CreateMunicipalityCommand(Repository<Municipality> municipalityRepository)
        {
            _municipalityRepository = municipalityRepository;
        }

        public void Execute(string name)
        {

            var municipality = Municipality.Create(name);
            _municipalityRepository.Save(municipality);
        }
    }
}