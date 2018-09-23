using System;
using System.Collections.Generic;

namespace EFCoreScaffoldexample.Model
{
    public partial class Collections
    {
        public Collections()
        {
            CollectionsFeeds = new HashSet<CollectionsFeeds>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int IdUser { get; set; }

        public Users IdUserNavigation { get; set; }
        public ICollection<CollectionsFeeds> CollectionsFeeds { get; set; }
    }
}
