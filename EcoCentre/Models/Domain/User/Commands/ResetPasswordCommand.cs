using System.Linq;

namespace EcoCentre.Models.Domain.User.Commands
{
    public class ResetPasswordCommand
    {
        private readonly Repository<User> _userRepository;

        public ResetPasswordCommand(Repository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public void Execute(ResetPasswordCommandViewModel model)
        {
            var user = _userRepository.Query.SingleOrDefault(x => x.EmailResetKey == model.Key);
            user.ResetPassword(model.Key, model.Password);
            _userRepository.Save(user);
        }
    }
}