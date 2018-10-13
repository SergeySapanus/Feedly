using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFeedlyServer.Contracts.Repositories.Entities;
using MyFeedlyServer.Entities;
using MyFeedlyServer.Entities.Entities;
using MyFeedlyServer.Entities.Extensions;

namespace MyFeedlyServer.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) :
            base(repositoryContext)
        {
        }

        public void CreateUser(User user)
        {
            Create(user);
            Save();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return FindAll()
                .OrderBy(u => u.Name);
        }

        public User GetUserById(int id)
        {
            return FindByCondition(u => u.Id.Equals(id))
             .FirstOrDefault();
        }

        public User GetUserByName(string name)
        {
            return FindByCondition(u => u.Name.Equals(name))
                .FirstOrDefault();
        }

        public void UpdateUser(User dbUser, User user)
        {
            dbUser.Map(user);
            Update(dbUser);
            Save();
        }

        public void DeleteUser(User user)
        {
            Delete(user);
            Save();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await FindAllAsync();
            return users.OrderBy(u => u.Name);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var users = await FindByConditionAync(u => u.Id.Equals(id));
            return users.FirstOrDefault();
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var users = await FindByConditionAync(u => u.Name.Equals(name));
            return users.FirstOrDefault();
        }

        public async Task CreateUserAsync(User user)
        {
            Create(user);
            await SaveAsync();
        }

        public async Task UpdateUserAsync(User dbUser, User user)
        {
            dbUser.Map(user);
            Update(dbUser);
            await SaveAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            Delete(user);
            await SaveAsync();
        }
    }
}