using System;
using System.Threading.Tasks;
using System.Xml;
using Entities.Contracts;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;

namespace SyndicationService
{
    static class SyndicationFactory
    {
        public static async Task<ISyndicationFeedReader> GetSyndicationFeedReader(IUriEntity uriEntity)
        {
            if (ReferenceEquals(uriEntity, null) || string.IsNullOrWhiteSpace(uriEntity.Uri))
                throw new ArgumentException(nameof(uriEntity));

            var syndicationFeedType = await GetSyndicationFeedType(uriEntity);

            switch (syndicationFeedType)
            {
                case SyndicationFeedType.Rss:
                    return new RssFeedReader(XmlReader.Create(uriEntity.Uri, new XmlReaderSettings { Async = true }));
                case SyndicationFeedType.Feed:
                    return new AtomFeedReader(XmlReader.Create(uriEntity.Uri, new XmlReaderSettings { Async = true }));
                default:
                    throw new UriFormatException(nameof(uriEntity));
            }
        }

        private static async Task<SyndicationFeedType> GetSyndicationFeedType(IUriEntity uriEntity)
        {
            if (ReferenceEquals(uriEntity, null) || string.IsNullOrWhiteSpace(uriEntity.Uri))
                throw new ArgumentException(nameof(uriEntity));

            using (var xmlReader = XmlReader.Create(uriEntity.Uri, new XmlReaderSettings { Async = true }))
            {
                for (var i = 0; i < 2; i++)
                {
                    if (await xmlReader.ReadAsync() && xmlReader.IsStartElement() && !xmlReader.IsEmptyElement)
                    {
                        if (Enum.TryParse(typeof(SyndicationFeedType), xmlReader.Name, true, out var result))
                            return (SyndicationFeedType)result;
                    }
                }
            }

            throw new ArgumentException(nameof(uriEntity));
        }
    }
}