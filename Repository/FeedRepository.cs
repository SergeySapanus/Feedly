using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class FeedRepository : RepositoryBase<Feed>, IFeedRepository
    {
        public FeedRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}