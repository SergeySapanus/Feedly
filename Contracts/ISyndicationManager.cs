using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Entities.Contracts;

namespace Contracts
{
    public interface ISyndicationManager
    {
        Task<List<News>> GetNews(IUriEntity uriEntity);
    }
}