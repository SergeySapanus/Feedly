using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyFeedlyServer.Entities.Contracts;
using MyFeedlyServer.Entities.Extensions;

namespace MyFeedlyServer.Entities.Entities
{
    [Table("News")]
    public class News : IUriEntity, IEntity
    {
        private Feed _feed;
        private readonly ILazyLoader _lazyLoader;

        public News()
        {
        }

        private News(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Uri { get; set; }

        public DateTimeOffset LastUpdated { get; set; }

        public DateTimeOffset Published { get; set; }

        public Feed Feed
        {
            get => _lazyLoader.Load(this, ref _feed);
            set => _feed = value;
        }

        public int FeedId { get; set; }

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