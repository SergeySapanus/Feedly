using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("News")]
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(Content) + " is required")]
        public string Content { get; set; }

        [Required(ErrorMessage = nameof(Feed) + " is required")]
        public virtual Feed Feed { get; set; }
    }
}