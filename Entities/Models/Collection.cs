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

        [Required(ErrorMessage = nameof(User) + " is required")]
        public virtual User User { get; set; }

        public virtual ICollection<CollectionFeed> CollectionsFeeds { get; set; } = new HashSet<CollectionFeed>();
    }
}
