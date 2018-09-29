using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Abstract;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Entities.Concrete
{
    [Table("CollectionsFeeds")]
    public class CollectionFeed : IEntity
    {
        private readonly ILazyLoader _lazyLoader;
        private Collection _collection;
        private Feed _feed;

        public CollectionFeed()
        {
        }

        private CollectionFeed(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        [Key]
        public int Id { get; set; }

        public Collection Collection
        {
            get => _lazyLoader.Load(this, ref _collection);
            set => _collection = value;
        }

        public int CollectionId { get; set; }

        public Feed Feed
        {
            get => _lazyLoader.Load(this, ref _feed);
            set => _feed = value;
        }

        public int FeedId { get; set; }
    }
}