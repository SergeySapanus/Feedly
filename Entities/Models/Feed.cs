using Entities.Abstracts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Feeds")]
    public class Feed : IEntity
    {
        private ILazyLoader _lazyLoader;
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

        [Required(ErrorMessage = nameof(Name) + " is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(Hash) + " is required")]
        public string Hash { get; set; }

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
    }
}