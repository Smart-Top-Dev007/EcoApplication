using System.Linq;
using EcoCentre.Models.Domain.Hubs;
using FluentValidation;

namespace EcoCentre.Models.Domain.User.Commands
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        private readonly Repository<User> _userRepository;

        public UserViewModelValidator(Repository<User> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Login)
                .NotEmpty()
                .Length(4,20)
                .OverridePropertyName(Resources.Model.User.Username);
            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(4,128)
                .OverridePropertyName(Resources.Model.User.Password);
            RuleFor(x => x.PasswordConfirmation)
                .Equal(x => x.Password)
                .WithMessage(Resources.Model.User.PasswordConfirmationSameAsPassword);
            RuleFor(x => x.Login)
                .Must(UniequeUserName)
                .When(x=>!string.IsNullOrEmpty(x.Login))
                .WithMessage(Resources.Model.User.UsernameExists);
            RuleFor(x => x.Email)
                .EmailAddress();
        }

	    private bool UniequeUserName(string login)
        {
            return !_userRepository.Query.Any(x => x.LoginLower == login.ToLower());
        }
    }
}