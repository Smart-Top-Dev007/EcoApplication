using FluentValidation;

namespace EcoCentre.Models.Domain.Invoices.Commands
{
    public class InvoiceMaterialViewModelValidator : AbstractValidator<InvoiceMaterialViewModel>
    {

        public InvoiceMaterialViewModelValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0);
        }

        
    }
}