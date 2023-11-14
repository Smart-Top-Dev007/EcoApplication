using System.Linq;
using FluentValidation;

namespace EcoCentre.Models.Domain.User.Commands
{
    public class ExistingUserViewModelValidator : AbstractValidator<ExistingUserViewModel>
    {
        private readonly Repository<User> _userRepository;
        private readonly AuthenticationContext _context;

        public ExistingUserViewModelValidator(Repository<User> userRepository, AuthenticationContext context)
        {
            _userRepository = userRepository;
            _context = context;

            RuleFor(x => x.Login)
                .NotEmpty()
                .Length(4,20)
                .OverridePropertyName(Resources.Model.User.Username);
            RuleFor(x => x.Password)
                .Length(4,128)
                .When(x=>!string.IsNullOrEmpty(x.Password))
                .OverridePropertyName(Resources.Model.User.Password);
            RuleFor(x => x.PasswordConfirmation)
                .Equal(x => x.Password)
                .When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage(Resources.Model.User.PasswordConfirmationSameAsPassword);
            RuleFor(x => x.Login)
                .Must(UniequeUserName)
                .When(x=>!string.IsNullOrEmpty(x.Login))
                .WithMessage(Resources.Model.User.UsernameExists);
            RuleFor(x => x.Email)
                .EmailAddress();
            RuleFor(x => x.Id)
                .Must(x =>
                {
                    var curUser = _userRepository.FindOne(x);
                    if (curUser.IsGlobalAdmin && !_context.User.IsGlobalAdmin)
                    {
                        return false;
                    }
                    return true;
                })
                .WithMessage(Resources.Model.User.AdminCantEditGlobalAdmin);
        }

        private bool UniequeUserName(ExistingUserViewModel vm,string login)
        {
            return !_userRepository.Query.Any(x => x.LoginLower == login.ToLower() && vm.Id != x.Id);
        }
    }
}