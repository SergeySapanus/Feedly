using System.Collections.Generic;
using System.Threading.Tasks;
using MyFeedlyServer.Entities.Contracts;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Contracts
{
    public interface ISyndicationManager
    {
        Task<List<News>> GetNews(IUriEntity uriEntity);
    }
}