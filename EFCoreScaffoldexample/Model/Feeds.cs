using System;
using System.Collections.Generic;

namespace EFCoreScaffoldexample.Model
{
    public partial class Feeds
    {
        public Feeds()
        {
            CollectionsFeeds = new HashSet<CollectionsFeeds>();
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }

        public ICollection<CollectionsFeeds> CollectionsFeeds { get; set; }
        public ICollection<News> News { get; set; }
    }
}
