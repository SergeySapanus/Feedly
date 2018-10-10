using System.ComponentModel.DataAnnotations;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Entities.Models
{
    public class UserCreateOrUpdateModel : EntityModel<User>
    {
        public UserCreateOrUpdateModel() : base()
        {
            Entity = new User();
        }

        public UserCreateOrUpdateModel(User user) : base(user)
        {
        }

        
        [Required(ErrorMessage = nameof(Name) + " is required")]
        public string Name
        {
            get => Entity.Name;
            set => Entity.Name = value;
        }

        [Required(ErrorMessage = nameof(Password) + " is required")]
        public string Password
        {
            get => Entity.Password;
            set => Entity.Password = value;
        }
    }
}
