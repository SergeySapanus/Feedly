using Contracts.Repositories.Entities;
using Entities;

namespace Repository
{
    public class NewsRepository : RepositoryBase<News>, INewsRepository
    {
        public NewsRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}