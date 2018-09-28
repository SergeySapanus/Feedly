using Entities.Models;
using System.Linq;
using System.Collections.Generic;

namespace MyFeedlyServer.Models
{
    public class UserWithCollectionsModel : UserWithNameModel
    {
        public UserWithCollectionsModel(User user) : base(user)
        {
        }

        public IEnumerable<CollectionModel> Colections => Source.Collections.Select(c => new CollectionModel(c));
    }
}
