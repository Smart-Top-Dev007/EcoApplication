using FluentValidation;

namespace EcoCentre.Models.Domain.User.Commands
{
    public class UpdateUserCommand
    {
        private readonly Repository<User> _userRepository;
        private readonly ExistingUserViewModelValidator _validator;

        public UpdateUserCommand(Repository<User> userRepository, ExistingUserViewModelValidator validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public void Execute(ExistingUserViewModel model)
        {
            _validator.ValidateAndThrow(model);
            var user = _userRepository.FindOne(model.Id);
            if(!string.IsNullOrEmpty(model.Password))
                user.UpdatePassword(model.Password);
            user.UpdateLogin(model.Login);
            user.IsReadOnly = model.IsReadOnly;
            user.IsAdmin = model.IsAdmin || model.IsGlobalAdmin;
            user.IsGlobalAdmin = model.IsGlobalAdmin;
            user.Email = model.Email;
            user.Name = model.Name;
	        user.IsLoginAlertEnabled = model.IsLoginAlertEnabled;
			_userRepository.Save(user);
        }
    }
}