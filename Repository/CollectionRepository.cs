using System.Collections.Generic;
using System.Linq;
using Contracts;
using Entities;
using Entities.Concrete;

namespace Repository
{
    public class CollectionRepository : RepositoryBase<Collection>, ICollectionRepository
    {
        public CollectionRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateCollection(Collection collection)
        {
            Create(collection);
            Save();
        }

        public Collection GetCollectionById(int id)
        {
            return FindByCondition(a => a.Id.Equals(id)).FirstOrDefault(); 
        }
    }
}