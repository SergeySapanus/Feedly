using MyFeedlyServer.Contracts.Repositories.Entities;
using MyFeedlyServer.Entities;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Repository
{
    public class CollectionFeedRepository : RepositoryBase<CollectionFeed>, ICollectionFeedRepository
    {
        public CollectionFeedRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateCollectionFeed(Collection collection, Feed feed)
        {
            var collectionFeed = new CollectionFeed { Collection = collection, Feed = feed };

            Create(collectionFeed);
            Save();
        }
    }
}