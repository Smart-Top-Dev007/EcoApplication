namespace EcoCentre.Models.Domain.User.Commands
{
    public class UserDeleteCommand
    {
        private readonly Repository<User> _userRepository;

        public UserDeleteCommand(Repository<User> userRepository )
        {
            _userRepository = userRepository;
        }

        public void Execute(string id)
        {
            var user = _userRepository.FindOne(id);
            if(user != null)
                _userRepository.Remove(user);
        }
    }
}