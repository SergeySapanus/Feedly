﻿using Entities.Models;

namespace MyFeedlyServer.Models
{
    public class CollectionModel : EntityModel<Collection>
    {
        public CollectionModel(Collection source) : base(source)
        {
        }

        public string Name => Source.Name;
    }
}