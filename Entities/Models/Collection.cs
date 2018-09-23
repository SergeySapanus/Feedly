using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Collections")]
    public class Collection
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Name) + " is required")]
        [StringLength(50, ErrorMessage = nameof(Name) + " can't be longer than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(Models.User) + " is required")]
        public User User { get; set; }

        public int UserId { get; set; }

        public ICollection<CollectionFeed> CollectionsFeeds { get; set; } = new HashSet<CollectionFeed>();
    }
}
