using MyApp.Core.Entities;
using MyApp.Core.Interfaces;

namespace MyApp.Core.UseCases
{
    public class CreateUser
    {
        private readonly IUserRepository _userRepository;

        public CreateUser(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Execute(User user)
        {
            _userRepository.Add(user);
        }
    }
}
