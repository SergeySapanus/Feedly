using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFeedlyServer.Models
{
    public class UserModel : NullModel<User>
    {
        public UserModel(User user) : base(user)
        {
        }

        public string Name => Source.Name;

        public string Password => Source.Password;

        public int ColectionsCount => Source.Collections.Count();
    }
}
