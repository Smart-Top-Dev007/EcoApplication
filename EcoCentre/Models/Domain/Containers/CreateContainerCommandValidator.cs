using EcoCentre.Models.Infrastructure;
using FluentValidation;

namespace EcoCentre.Models.Domain.Containers
{
	public class CreateContainerCommandValidator : AbstractValidator<CreateContainerCommand>
	{
		public CreateContainerCommandValidator()
		{
			RuleFor(s => s.MaterialIds)
				.NotEmptyCollection();

			RuleFor(s => s.Capacity)
				.GreaterThan(0);

			RuleFor(s => s.Number)
				.NotEmpty();
		}
	}
}