using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repositories.Entities;
using MyFeedlyServer.Contracts;
using MyFeedlyServer.Entities;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Repository
{
    public class FeedRepository : RepositoryBase<Feed>, IFeedRepository
    {
        public FeedRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Feed> GetAllFeeds()
        {
            return FindAll()
                .OrderBy(f => f.Uri);
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

        public void AddNews(Feed feed, IEnumerable<News> news)
        {
            foreach (var n in news)
                feed.News.Add(n);

            Update(feed);
            Save();
        }

        public IEnumerable<News> GetNewsByCollection(Collection collection, ISyndicationManager syndicationManager)
        {
            var news = collection.CollectionsFeeds.SelectMany(cf => GetNewsByFeed(cf.Feed, syndicationManager).Result);

            return news;
        }

        private async Task<IEnumerable<News>> GetNewsByFeed(Feed feed, ISyndicationManager syndicationManager)
        {
            if (feed.News.Count > 0)
                return feed.News;

            var news = await syndicationManager.GetNews(feed);

            await Task.Run(() => { AddNews(feed, news); });

            return news;
        }
    }
}