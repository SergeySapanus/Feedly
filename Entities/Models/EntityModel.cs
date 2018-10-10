using MyFeedlyServer.Entities.Contracts;

namespace MyFeedlyServer.Entities.Models
{
    public class EntityModel<T> where T : IEntity
    {
        protected EntityModel()
        {
        }

        public EntityModel(T entity)
        {
            Entity = entity;
        }

        public int Id
        {
            get => Entity.Id;
            set => Entity.Id = value;
        }

        protected T Entity { get; set; }

        public bool IsNull() => ReferenceEquals(Entity, null);

        public virtual T GetEntity() => Entity;
    }
}
