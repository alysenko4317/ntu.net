using MyApp.Core.Interfaces;
using MyApp.Core.Entities;

namespace MyApp.Presentation
{
    public class ConsoleService
    {
        private readonly IUserRepository _userRepository;

        public ConsoleService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public void DisplayAllUsers()
        {
            List<User> users = _userRepository.GetAllUsers();

            if (users.Count() > 0) {
                foreach (var user in users)
                    Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Email: {user.Email}");
            } else {
                Console.WriteLine("No users found.");
            }
        }
    }
}