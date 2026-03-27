
using MyApp.Core.Entities;

namespace MyApp.Core.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        User FindById(int id);
        List<User> GetAllUsers();
    }
}
