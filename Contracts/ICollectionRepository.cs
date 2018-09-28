using Entities.Models;
using System.Collections.Generic;

namespace Contracts
{
    public interface ICollectionRepository : IRepositoryBase<Collection>
    {
        IEnumerable<Collection> CollectionsByUser(int userId);
        Collection GetCollectionById(int id);

        void CreateCollection(Collection collection);
    }
}