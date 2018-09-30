using System.Collections.Generic;
using System.Linq;

namespace Entities.Models
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

        public IEnumerable<NewsModel> News => Entity.News.Select(n => new NewsModel(n));
    }
}