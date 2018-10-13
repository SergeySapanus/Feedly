using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Contracts.Repositories.Entities
{
    public interface ICollectionFeedRepository : IRepositoryBase<CollectionFeed>
    {
        void CreateCollectionFeed(Collection collection, Feed feed);
    }
}