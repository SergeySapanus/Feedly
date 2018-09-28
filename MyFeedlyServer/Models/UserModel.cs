using Entities.Models;

namespace MyFeedlyServer.Models
{
    public class UserModel : EntityModel<User>
    {
        public UserModel(User user) : base(user)
        {
        }

        public string Name => Source.Name;

        public string Password => Source.Password;

        public int ColectionsCount => Source.Collections.Count;
    }
}
