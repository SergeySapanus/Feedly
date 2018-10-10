using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyFeedlyServer.Entities.Contracts;
using MyFeedlyServer.Entities.Extensions;

namespace MyFeedlyServer.Entities.Entities
{
    [Table("Feeds")]
    public class Feed : IUriEntity, IEntity
    {
        private readonly ILazyLoader _lazyLoader;
        private ICollection<CollectionFeed> _collectionsFeeds = new HashSet<CollectionFeed>();
        private ICollection<News> _news = new HashSet<News>();

        public Feed()
        {
        }

        private Feed(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        [Key]
        public int Id { get; set; }

        public string Uri { get; set; }

        public int Hash { get; set; }

        public ICollection<CollectionFeed> CollectionsFeeds
        {
            get => _lazyLoader.Load(this, ref _collectionsFeeds);
            set => _collectionsFeeds = value;
        }

        public ICollection<News> News
        {
            get => _lazyLoader.Load(this, ref _news);
            set => _news = value;
        }

        public override int GetHashCode()
        {
            return this.GetUriHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.UriEquals(obj);
        }
    }
}