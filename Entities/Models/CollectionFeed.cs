using Entities.Abstracts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("CollectionsFeeds")]
    public class CollectionFeed : IEntity
    {
        private ILazyLoader _lazyLoader;
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

        [Required(ErrorMessage = nameof(Collection) + " is required")]
        public int CollectionId { get; set; }

        public Feed Feed
        {
            get => _lazyLoader.Load(this, ref _feed);
            set => _feed = value;
        }

        [Required(ErrorMessage = nameof(Feed) + " is required")]
        public int FeedId { get; set; }
    }
}