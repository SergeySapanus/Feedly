using System;
using System.Collections.Generic;

namespace EFCoreScaffoldexample.Model
{
    public partial class Users
    {
        public Users()
        {
            Collections = new HashSet<Collections>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public ICollection<Collections> Collections { get; set; }
    }
}
