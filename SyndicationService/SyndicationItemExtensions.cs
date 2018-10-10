using System.Linq;
using Microsoft.SyndicationFeed;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.SyndicationService
{
    static class SyndicationItemExtensions
    {
        public static News GetNews(this ISyndicationItem syndicationItem)
        {
            var result = new News();
            if (ReferenceEquals(syndicationItem, null))
                return result;

            result.Title = syndicationItem.Title ?? string.Empty;
            result.Description = syndicationItem.Description ?? string.Empty;
            result.Uri = syndicationItem.Links.FirstOrDefault()?.Uri?.AbsoluteUri ?? string.Empty;
            result.LastUpdated = syndicationItem.LastUpdated;
            result.Published = syndicationItem.Published;

            return result;
        }
    }
}