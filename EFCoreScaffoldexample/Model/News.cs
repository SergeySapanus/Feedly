using System;
using System.Collections.Generic;

namespace EFCoreScaffoldexample.Model
{
    public partial class News
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int IdFeed { get; set; }

        public Feeds IdFeedNavigation { get; set; }
    }
}
