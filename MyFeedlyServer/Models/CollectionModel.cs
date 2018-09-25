using Entities.Models;

namespace MyFeedlyServer.Models
{
    public class CollectionModel : NullModel<Collection>
    {
        public CollectionModel(Collection source) : base(source)
        {
        }

        public int Id => Source.Id;

        public string Name => Source.Name;

        public int UserId => Source.UserId;
    }
}