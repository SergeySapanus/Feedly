using Entities;

namespace Contracts.Repositories.Entities
{
    public interface ICollectionFeedRepository : IRepositoryBase<CollectionFeed>
    {
        void CreateCollectionFeed(Collection collection, Feed feed);
    }
}