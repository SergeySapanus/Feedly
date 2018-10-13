using System.Collections.Generic;
using System.Threading.Tasks;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Contracts.Repositories.Entities
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByName(string name);

        void CreateUser(User user);
        void UpdateUser(User dbUser, User user);
        void DeleteUser(User user);

        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string name);

        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User dbUser, User user);
        Task DeleteUserAsync(User user);
    }
}