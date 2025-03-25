using MyApp.Core.Entities;
using MyApp.Core.Interfaces;
using MyApp.Framework.Database;

namespace MyApp.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _dbContext;

        public UserRepository(DatabaseContext dbContext) {
            _dbContext = dbContext;
        }

        void IUserRepository.Add(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        User IUserRepository.FindById(int id) {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        List<User> IUserRepository.GetAllUsers() {
            return _dbContext.Users.ToList();
        }
    }
}
