using System.Linq;
using FluentValidation;

namespace EcoCentre.Models.Domain.Municipalities
{
	public class MunicipalityValidator : AbstractValidator<Municipality>
	{
		private readonly Repository<Municipality> _municipalityRepository;

		public MunicipalityValidator(Repository<Municipality> municipalityRepository )
		{
			_municipalityRepository = municipalityRepository;
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Name).Must(UniqueMunicipality).WithMessage("");
		}

		private bool UniqueMunicipality(Municipality m,string name)
		{
			return !_municipalityRepository.Query.Any(x => x.Id != m.Id && x.Name == name);
		}
	}
}