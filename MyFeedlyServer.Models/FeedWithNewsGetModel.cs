using System.Collections.Generic;
using System.Linq;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Models
{
    public class FeedWithNewsGetModel : FeedGetModel
    {
        public FeedWithNewsGetModel() : base()
        {
            Entity = new Feed();
        }

        public FeedWithNewsGetModel(Feed entity) : base(entity)
        {
        }

        public IEnumerable<NewsGetModel> News => Entity.News.Select(n => new NewsGetModel(n));
    }
}