using System.Collections.Generic;
using System.Linq;
using Entities.Concrete;

namespace Entities.Model
{
    public class CollectionGetModel : EntityModel<Collection>
    {
        public CollectionGetModel() : base()
        {
            Entity = new Collection();
        }

        public CollectionGetModel(Collection entity) : base(entity)
        {
        }

        public string Name => Entity.Name;

        public IEnumerable<FeedGetModel> Feeds => Entity.CollectionsFeeds.Select(cf => new FeedGetModel(cf.Feed));
    }
}