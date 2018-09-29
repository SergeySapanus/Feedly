using Entities.Concrete;

namespace Entities.Model
{
    public class FeedGetModel : EntityModel<Feed>
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