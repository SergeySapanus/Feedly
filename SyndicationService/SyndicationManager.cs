using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SyndicationFeed;
using MyFeedlyServer.Contracts;
using MyFeedlyServer.Entities.Contracts;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.SyndicationService
{
    public class SyndicationManager : ISyndicationManager
    {
        private readonly ILoggerManager _logger;

        public SyndicationManager(ILoggerManager logger)
        {
            _logger = logger;
        }

        public async Task<List<News>> GetNews(IUriEntity uriEntity)
        {
            if (string.IsNullOrWhiteSpace(uriEntity.Uri))
                return null;

            var items = new HashSet<ISyndicationItem>();

            try
            {
                var feedReader = await SyndicationFactory.GetSyndicationFeedReader(uriEntity);

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
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong: {exception.InnerException?.Message ?? exception.Message}");
            }

            return items.Select(i => i.GetNews()).ToList();
        }
    }
}
