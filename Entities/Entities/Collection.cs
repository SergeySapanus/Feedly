using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Contracts;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Entities
{
    [Table("Collections")]
    public class Collection: IEntity
    {
        private User _user;
        private ICollection<CollectionFeed> _collectionsFeeds = new HashSet<CollectionFeed>();
        private readonly ILazyLoader _lazyLoader;

        public Collection()
        {
        }

        private Collection(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public User User
        {
            get => _lazyLoader.Load(this, ref _user);
            set => _user = value;
        }

        public int UserId { get; set; }


        public ICollection<CollectionFeed> CollectionsFeeds
        {
            get => _lazyLoader.Load(this, ref _collectionsFeeds);
            set => _collectionsFeeds = value;
        }
    }
}
