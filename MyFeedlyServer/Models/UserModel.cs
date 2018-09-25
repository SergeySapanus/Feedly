using Entities.Models;

namespace MyFeedlyServer.Models
{
    public class UserModel : NullModel<User>
    {
        public UserModel(User user) : base(user)
        {
        }

        public int Id => Source.Id;

        public string Name => Source.Name;

        public string Password => Source.Password;

        public int ColectionsCount => Source.Collections.Count;
    }
}
