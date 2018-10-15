using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Models
{
    public class FeedGetModel : EntityGetModel<Feed>
    {
        public FeedGetModel() : base()
        {
            Entity = new Feed();
        }

        public FeedGetModel(Feed entity) : base(entity)
        {
        }

        public string Uri => Entity.Uri;

        public int Hash => Entity.Hash;
    }
}