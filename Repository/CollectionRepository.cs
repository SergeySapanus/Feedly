using System.Linq;
using MyFeedlyServer.Contracts.Repositories.Entities;
using MyFeedlyServer.Entities;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Repository
{
    public class CollectionRepository : RepositoryBase<Collection>, ICollectionRepository
    {
        public CollectionRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Collection GetCollectionById(int id)
        {
            return FindByCondition(a => a.Id.Equals(id)).FirstOrDefault();
        }

        public Collection GetCollectionByIdAndUserId(int id, int userId)
        {
            return FindByCondition(a => a.Id.Equals(id) && a.UserId.Equals(userId)).FirstOrDefault();
        }

        public void CreateCollection(Collection collection)
        {
            Create(collection);
            Save();
        }
    }
}