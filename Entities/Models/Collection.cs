using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Entities.Models
{
    [Table("Collections")]
    public class Collection
    {
        private User _user;
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

        [Required(ErrorMessage = nameof(Name) + " is required")]
        [StringLength(50, ErrorMessage = nameof(Name) + " can't be longer than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(User) + " is required")]
        public User User
        {
            get => _lazyLoader.Load(this, ref _user);
            set => _user = value;
        }

        public int UserId { get; set; }

        public virtual ICollection<CollectionFeed> CollectionsFeeds { get; set; } = new HashSet<CollectionFeed>();
    }
}
