using FluentValidation;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Commands
{
    public class OBNLReinvestmentMaterialViewModelValidator : AbstractValidator<OBNLReinvestmentMaterialViewModel>
    {

        public OBNLReinvestmentMaterialViewModelValidator()
        {
            RuleFor(x => x.Weight).GreaterThan(0);
        }

        
    }
}