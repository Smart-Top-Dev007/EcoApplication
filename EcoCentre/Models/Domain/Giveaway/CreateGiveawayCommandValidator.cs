using FluentValidation;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class CreateGiveawayCommandValidator : AbstractValidator<CreateGiveawayCommand>
	{
		public CreateGiveawayCommandValidator()
		{
			RuleFor(s => s.Type)
				.NotEmpty();
		}
	}
}