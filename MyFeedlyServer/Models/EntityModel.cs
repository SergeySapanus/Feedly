using Entities.Abstracts;

namespace MyFeedlyServer.Models
{
    public class EntityModel<T> : NullModel<T> where T : IEntity
    {
        public EntityModel(T source) : base(source)
        {
        }

        public int Id => Source.Id;
    }
}
