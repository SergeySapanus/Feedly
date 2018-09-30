using System.Collections.Generic;
using Contracts.Repositories;
using Entities;

namespace Contracts.Repositories.Entities
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);

        void CreateUser(User user);
        void UpdateUser(User dbUser, User user);
        void DeleteUser(User user);
    }
}