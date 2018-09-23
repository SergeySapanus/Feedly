using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CollectionFeedRepository : RepositoryBase<CollectionFeed>, ICollectionFeedRepository
    {
        public CollectionFeedRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}