using Contracts.Repositories.Entities;
using MyFeedlyServer.Entities;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Repository
{
    public class NewsRepository : RepositoryBase<News>, INewsRepository
    {
        public NewsRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}