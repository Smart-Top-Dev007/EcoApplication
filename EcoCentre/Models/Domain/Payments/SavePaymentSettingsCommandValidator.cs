using System.Text.RegularExpressions;
using FluentValidation;

namespace EcoCentre.Models.Domain.Payments
{
	public class SavePaymentSettingsCommandValidator : AbstractValidator<SavePaymentSettingsCommand>
	{
		public SavePaymentSettingsCommandValidator()
		{
			RuleFor(s => s.Tpe).NotEmpty();
			RuleFor(s => s.Company).NotEmpty();
			RuleFor(s => s.Currency).NotEmpty();
			RuleFor(s => s.Key).NotEmpty();
			RuleFor(s => s.Language).NotEmpty();

			RuleFor(s => s.Key).Length(40);
			RuleFor(s => s.Key).Matches(new Regex("^[0-9A-Fa-f]{40}$"));

		}
	}
}