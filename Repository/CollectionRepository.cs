using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CollectionRepository : RepositoryBase<Collection>, ICollectionRepository
    {
        public CollectionRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}