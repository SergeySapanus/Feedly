using MyFeedlyServer.Entities.Contracts;
using Newtonsoft.Json;

namespace MyFeedlyServer.Models
{
    public class EntityGetModel : EntityModel<IEntity>
    {
        private readonly int _id = -1;

        public EntityGetModel(IEntity entity) : base(entity)
        {
        }

        public EntityGetModel(int id) : base()
        {
            _id = id;
        }


        public new int Id => Entity?.Id ?? _id;
    }
}