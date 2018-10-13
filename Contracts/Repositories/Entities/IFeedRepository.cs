using System.Collections.Generic;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Contracts.Repositories.Entities
{
    public interface IFeedRepository : IRepositoryBase<Feed>
    {
        IEnumerable<Feed> GetAllFeeds();
        Feed GetFeedById(int id);
        Feed GetFeedByHash(int hash);

        void CreateFeed(Collection collection, Feed feed);
        void AddNews(Feed feed, IEnumerable<News> news);

        IEnumerable<News> GetNewsByCollection(Collection collection, ISyndicationManager syndicationManager);
    }
}