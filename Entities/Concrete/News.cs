using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Abstract;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Entities.Concrete
{
    [Table("News")]
    public class News : IEntity
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

        public string Content { get; set; }

        public Feed Feed
        {
            get => _lazyLoader.Load(this, ref _feed);
            set => _feed = value;
        }

        public int FeedId { get; set; }
    }
}