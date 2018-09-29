using Entities.Concrete;

namespace Contracts
{
    public interface IFeedRepository : IRepositoryBase<Feed>
    {
        Feed GetFeedById(int id);
        Feed GetFeedByHash(int hash);

        void CreateFeed(Collection collection, Feed feed);
    }
}