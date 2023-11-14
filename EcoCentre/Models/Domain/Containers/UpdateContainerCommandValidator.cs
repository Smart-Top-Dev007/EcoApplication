using EcoCentre.Models.Infrastructure;
using FluentValidation;

namespace EcoCentre.Models.Domain.Containers
{
	public class UpdateContainerCommandValidator : AbstractValidator<UpdateContainerCommand>
	{
		public UpdateContainerCommandValidator()
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