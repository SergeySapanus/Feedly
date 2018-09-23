using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = nameof(Name) + " is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = nameof(Password) + " is required")]
        public string Password { get; set; }

        //public ICollection<Collection> Collections { get; set; }
    }
}