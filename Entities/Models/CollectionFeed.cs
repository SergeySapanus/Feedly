using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("CollectionsFeeds")]
    public class CollectionFeed
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Collection) + " is required")]
        public Collection Collection { get; set; }

        [Required(ErrorMessage = nameof(Feed) + " is required")]
        public Feed Feed { get; set; }
    }
}