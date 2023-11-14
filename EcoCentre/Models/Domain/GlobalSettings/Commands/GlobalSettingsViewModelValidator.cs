using System;
using System.Net.Mail;
using FluentValidation;

namespace EcoCentre.Models.Domain.GlobalSettings.Commands
{
    public class GlobalSettingsViewModelValidator : AbstractValidator<GlobalSettingsViewModel>
    {
        public GlobalSettingsViewModelValidator()
        {            
            RuleFor(s => s.MaxYearlyClientVisits)
                .GreaterThanOrEqualTo(0)
                .OverridePropertyName("MaxYearlyClientVisits")
                .WithMessage(Resources.Model.GlobalSettings.MaxYearlyClientVisitsInvalid);

            RuleFor(s => s.MaxYearlyClientVisitsWarning)
                .LessThanOrEqualTo(s => s.MaxYearlyClientVisits)
                .GreaterThanOrEqualTo(0)
                .OverridePropertyName("MaxYearlyClientVisitsWarning")
                .WithMessage(Resources.Model.GlobalSettings.MaxYearlyClientVisitsWarningInvalid);

            RuleFor(s => s.AdminNotificationsEmail)
                .Must(IsEmailValid)
                .WithMessage(Resources.Model.GlobalSettings.AdminNotificationsEmailInvalid);

	        RuleFor(s => s.ContainerFullNotificationsEmail)
		        .EmailAddress()
				.When(x=>string.IsNullOrEmpty(x.AdminNotificationsEmail));

	        RuleFor(s => s.QstTaxRate)
		        .Must(x => x >= 0 && x < 100);

			RuleFor(s => s.GstTaxRate)
		        .Must(x => x >= 0 && x < 100);

		}

        private bool IsEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email)) return true;
            try
            {
                MailAddress _ = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}