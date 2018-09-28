using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace Repository
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
    }
}