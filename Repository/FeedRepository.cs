using System.Linq;
using Contracts;
using Entities;
using Entities.Concrete;

namespace Repository
{
    public class FeedRepository : RepositoryBase<Feed>, IFeedRepository
    {
        public FeedRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public Feed GetFeedById(int id)
        {
            return FindByCondition(u => u.Id.Equals(id))
                .FirstOrDefault();
        }

        public Feed GetFeedByHash(int hash)
        {
            return FindByCondition(u => u.Hash.Equals(hash))
                .FirstOrDefault();
        }

        public void CreateFeed(Collection collection, Feed feed)
        {
            feed.CollectionsFeeds.Add(new CollectionFeed { Collection = collection, Feed = feed });

            if (feed.Hash == 0)
                feed.Hash = feed.GetHashCode();

            Create(feed);
            Save();
        }
    }
}