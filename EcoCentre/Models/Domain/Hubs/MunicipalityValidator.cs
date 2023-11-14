using FluentValidation;

namespace EcoCentre.Models.Domain.Hubs
{
	public class HubValidator : AbstractValidator<Hub>
	{
		public HubValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
		}
	}
}