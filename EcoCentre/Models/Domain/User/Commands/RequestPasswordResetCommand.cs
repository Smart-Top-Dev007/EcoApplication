using System.Linq;

namespace EcoCentre.Models.Domain.User.Commands
{
    public class RequestPasswordResetCommand
    {
        private readonly Repository<User> _userRepository;

        public RequestPasswordResetCommand(Repository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public string LoginOrEmail { get; set; }

        public PasswordResetCommandResult Execute()
        {
            LoginOrEmail = LoginOrEmail.ToLower();
            var user =
                _userRepository.Query.SingleOrDefault(x => x.Email == LoginOrEmail || x.LoginLower == LoginOrEmail);
            if (user == null)
                return PasswordResetCommandResult.UserNotFound;
            if (string.IsNullOrEmpty(user.Email))
                return PasswordResetCommandResult.NoEmail;
            var mailer = new RequestPasswordResetMailer();
            user.ResetPasswordRequest();
            _userRepository.Save(user);
            using (var mail = mailer.Create(user))
                mail.Send();

            return PasswordResetCommandResult.Success;

        }
    }

}