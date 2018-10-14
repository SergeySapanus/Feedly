using MyFeedlyServer.Entities.Contracts;
using Newtonsoft.Json;

namespace MyFeedlyServer.Models
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

        [JsonIgnore]
        public int Id
        {
            get => Entity.Id;
            set => Entity.Id = value;
        }

        protected T Entity { get; set; }

        public bool IsNullEntity() => ReferenceEquals(Entity, null);

        public virtual T GetEntity() => Entity;
    }
}
