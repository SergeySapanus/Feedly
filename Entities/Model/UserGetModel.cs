using Entities.Concrete;

namespace Entities.Model
{
    public class UserGetModel : EntityModel<User>
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
