using System.Collections.Generic;
using System.Linq;
using Contracts.Repositories.Entities;
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
    }
}