using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class CollectionCreateOrUpdateModel : EntityModel<Collection>
    {
        public CollectionCreateOrUpdateModel() : base()
        {
            Entity = new Collection();
        }

        public CollectionCreateOrUpdateModel(Collection entity) : base(entity)
        {
        }

        [Required(ErrorMessage = nameof(Name) + " is required")]
        [StringLength(50, ErrorMessage = nameof(Name) + " can't be longer than 50 characters")]
        public string Name
        {
            get => Entity.Name;
            set => Entity.Name = value;
        }

        [Required(ErrorMessage = nameof(UserId) + " is required")]
        public int UserId
        {
            get => Entity.UserId;
            set => Entity.UserId = value;
        }
    }
}