using FluentValidation;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class UpdateGiveawayCommandValidator : AbstractValidator<UpdateGiveawayCommand>
	{
		public UpdateGiveawayCommandValidator()
		{
			RuleFor(s => s.Type)
				.NotEmpty();
			RuleFor(s => s.Id)
				.NotEmpty();
		}
	}
}