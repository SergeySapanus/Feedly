using System.Collections.Generic;
using System.Linq;

namespace Entities.Models
{
    public class UserWithCollectionsGetModel : UserGetModel
    {
        public UserWithCollectionsGetModel(User entity) : base(entity)
        {
        }

        public IEnumerable<CollectionGetModel> Colections => Entity.Collections.Select(c => new CollectionGetModel(c));
    }
}
