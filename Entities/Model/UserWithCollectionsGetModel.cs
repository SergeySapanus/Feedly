using System.Collections.Generic;
using System.Linq;
using Entities.Concrete;

namespace Entities.Model
{
    public class UserWithCollectionsGetModel : UserGetModel
    {
        public UserWithCollectionsGetModel(User entity) : base(entity)
        {
        }

        public IEnumerable<CollectionGetModel> Colections => Entity.Collections.Select(c => new CollectionGetModel(c));
    }
}
