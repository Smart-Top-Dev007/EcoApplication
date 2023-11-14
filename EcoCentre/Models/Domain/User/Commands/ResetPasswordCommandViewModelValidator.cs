using System.Linq;
using FluentValidation;

namespace EcoCentre.Models.Domain.User.Commands
{
    public class ResetPasswordCommandViewModelValidator : AbstractValidator<ResetPasswordCommandViewModel>
    {
        private readonly Repository<User> _userRepository;

        public ResetPasswordCommandViewModelValidator(Repository<User> userRepository )
        {
            _userRepository = userRepository;
            RuleFor(x => x.Key)
                .NotEmpty()
                .Must(BeValidResetKey)
                .WithMessage("Demande de réinitialisation de mot n'est plus valide");
            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 128)
                .OverridePropertyName(Resources.Model.User.Password)
                .When(x=>x.Reseting);
            RuleFor(x => x.PasswordConfirmation)
                .Equal(x => x.Password)
                .WithMessage(Resources.Model.User.PasswordConfirmationSameAsPassword)
                .When(x => x.Reseting);
        }

        private bool BeValidResetKey(string key)
        {
            return _userRepository.Query.Any(x => x.EmailResetKey == key);
        }
    }
}