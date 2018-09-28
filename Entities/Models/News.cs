using Entities.Abstracts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("News")]
    public class News : IEntity
    {
        private Feed _feed;
        private ILazyLoader _lazyLoader;

        public News()
        {
        }

        private News(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Content) + " is required")]
        public string Content { get; set; }

        public Feed Feed
        {
            get => _lazyLoader.Load(this, ref _feed);
            set => _feed = value;
        }

        [Required(ErrorMessage = nameof(Feed) + " is required")]
        public int FeedId { get; set; }
    }
}