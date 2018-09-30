using System.Collections.Generic;
using Entities;

namespace Contracts.Repositories.Entities
{
    public interface IFeedRepository : IRepositoryBase<Feed>
    {
        Feed GetFeedById(int id);
        Feed GetFeedByHash(int hash);

        void CreateFeed(Collection collection, Feed feed);
        void AddNews(Feed feed, IEnumerable<News> news);

        IEnumerable<News> GetNewsByCollection(Collection collection, ISyndicationManager syndicationManager);
    }
}