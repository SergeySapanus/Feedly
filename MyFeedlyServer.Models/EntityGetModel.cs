using MyFeedlyServer.Entities.Contracts;

namespace MyFeedlyServer.Models
{
    public class EntityGetModel<T> : EntityModel<T> where T : IEntity
    {
        private readonly int _id = -1;

        public EntityGetModel()
        {
        }

        public EntityGetModel(T entity) : base(entity)
        {
        }

        public EntityGetModel(int id) : base()
        {
            _id = id;
        }

        public new int Id => Entity?.Id ?? _id;
    }
}