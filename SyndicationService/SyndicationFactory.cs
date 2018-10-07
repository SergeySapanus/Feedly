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
                case SyndicationFeedType.Atom:
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
                        if (xmlReader.HasAttributes)
                        {
                            for (var j = 0; j < xmlReader.AttributeCount; j++)
                            {
                                var result = GetSyndicationFeedTypeFromAttribute(xmlReader.GetAttribute(j));
                                if (result != SyndicationFeedType.None)
                                    return result;
                            }
                        }
                        else
                        {
                            var result = GetSyndicationFeedTypeFromElement(xmlReader.Name);
                            if (result != SyndicationFeedType.None)
                                return result;
                        }
                    }
                }
            }

            throw new ArgumentException(nameof(uriEntity));
        }

        private static SyndicationFeedType GetSyndicationFeedTypeFromAttribute(string attributeValue)
        {
            switch (attributeValue)
            {
                case "http://www.w3.org/2005/Atom":
                    return SyndicationFeedType.Atom;
                case "http://purl.org/dc/elements/1.1/":
                    return SyndicationFeedType.Rss;
                default:
                    return SyndicationFeedType.None;
            }
        }

        private static SyndicationFeedType GetSyndicationFeedTypeFromElement(string elementName)
        {
            switch (elementName)
            {
                case "feed":
                    return SyndicationFeedType.Atom;
                case "rss":
                    return SyndicationFeedType.Rss;
                default:
                    return SyndicationFeedType.None;
            }
        }
    }
}