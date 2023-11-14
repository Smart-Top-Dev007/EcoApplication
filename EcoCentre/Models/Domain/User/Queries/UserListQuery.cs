using System.Collections.Generic;
using System.Linq;

namespace EcoCentre.Models.Domain.User.Queries
{
    public class UserListQuery
    {
        private readonly Repository<User> _userRepository;

        public UserListQuery(Repository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> Execute()
        {
            return _userRepository.Query.ToList();
        }
    }
}