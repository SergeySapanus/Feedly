using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Contracts;
using Microsoft.SyndicationFeed;

namespace SyndicationService
{
    public class SyndicationManager : ISyndicationManager
    {
        public async Task<List<News>> GetNews(IUriEntity uriEntity)
        {
            if (string.IsNullOrWhiteSpace(uriEntity.Uri))
                return null;

            var feedReader = await SyndicationFactory.GetSyndicationFeedReader(uriEntity);

            var items = new HashSet<ISyndicationItem>();

            while (await feedReader.Read())
            {
                switch (feedReader.ElementType)
                {
                    case SyndicationElementType.Item:
                        var item = await feedReader.ReadItem();
                        items.Add(item);
                        break;
                }
            }

            return items.Select(i => i.GetNews()).ToList();
        }
    }
}
