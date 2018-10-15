using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Models
{
    public class UserGetModel : EntityGetModel<User>
    {
        public UserGetModel() : base()
        {
            Entity = new User();
        }

        public UserGetModel(User entity) : base(entity)
        {
        }

        public string Name => Entity.Name;
    }
}
