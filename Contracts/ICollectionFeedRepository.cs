using Entities.Concrete;

namespace Contracts
{
    public interface ICollectionFeedRepository : IRepositoryBase<CollectionFeed>
    {
        void CreateCollectionFeed(Collection collection, Feed feed);
    }
}