using System;
using System.Collections.Generic;

namespace EFCoreScaffoldexample.Model
{
    public partial class CollectionsFeeds
    {
        public int Id { get; set; }
        public int IdCollection { get; set; }
        public int IdFeed { get; set; }

        public Collections IdCollectionNavigation { get; set; }
        public Feeds IdFeedNavigation { get; set; }
    }
}
