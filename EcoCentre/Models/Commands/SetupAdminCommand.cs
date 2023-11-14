using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.User;

namespace EcoCentre.Models.Commands
{
    public class SetupAdminCommand
    {
        private readonly Repository<User> _userRepository;

        public SetupAdminCommand(Repository<User> userRepository )
        {
            _userRepository = userRepository;
        }

        public void Execute(string login, string password, string email)
        {
            var user = User.Create(login, password, email);
            user.MakeGlobalAdmin();
            _userRepository.Save(user);
        }
    }
}