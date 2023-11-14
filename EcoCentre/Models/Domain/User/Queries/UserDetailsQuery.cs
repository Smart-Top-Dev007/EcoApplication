namespace EcoCentre.Models.Domain.User.Queries
{
    public class UserDetailsQuery
    {
        private readonly Repository<User> _userRepository;

        public UserDetailsQuery(Repository<User> userRepository )
        {
            _userRepository = userRepository;
        }

        public User Execute(string id)
        {
            return _userRepository.FindOne(id);
        }
    }
}