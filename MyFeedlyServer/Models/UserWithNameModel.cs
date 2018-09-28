using Entities.Models;

namespace MyFeedlyServer.Models
{
    public class UserWithNameModel : EntityModel<User>
    {
        public UserWithNameModel(User source) : base(source)
        {
        }

        public string Name => Source.Name;
    }
}
