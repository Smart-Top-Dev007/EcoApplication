using FluentValidation;

namespace EcoCentre.Models.Domain.User.Commands
{
    public class CreateUserCommand
    {
        private readonly Repository<User> _userRepository;
        private readonly UserViewModelValidator _validator;
        private readonly AuthenticationContext _context;

        public CreateUserCommand(Repository<User> userRepository, UserViewModelValidator validator, AuthenticationContext context)
        {
            _userRepository = userRepository;
            _validator = validator;
            _context = context;
        }

        public void Execute(UserViewModel model)
        {
            _validator.ValidateAndThrow(model);
            var user = User.Create(model.Login, model.Password, model.Email);
            if (model.IsGlobalAdmin && _context.User.IsGlobalAdmin) //only global admin can change this
            {
                user.MakeGlobalAdmin();
            }
            else if (model.IsAdmin)
            {
                user.MakeAdmin();
            }
            user.IsReadOnly = model.IsReadOnly;
	        user.IsLoginAlertEnabled = model.IsLoginAlertEnabled;
			user.Name = model.Name;
            _userRepository.Save(user);
        }
    }

}