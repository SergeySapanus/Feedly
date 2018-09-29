using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Entities.Abstract;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Entities.Concrete
{
    [Table("Feeds")]
    public class Feed : IEntity
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
            var uri = GetUri();

            if (string.IsNullOrWhiteSpace(uri))
                return 0;

            var bytes = Encoding.Unicode.GetBytes(uri);
            var sum = 0;

            foreach (var @byte in bytes)
                for (var i = 0; i < 8; i++)
                    sum += (@byte >> i) & 1;

            return sum;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Feed feed))
                return false;

            if (ReferenceEquals(this, feed))
                return true;

            if (Uri.Length != feed.Uri.Length)
                return false;

            return Uri == feed.Uri;
        }

        private string GetUri()
        {
            return Uri;
        }
    }
}