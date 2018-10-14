using System;
using System.ComponentModel.DataAnnotations;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Models
{
    public class NewsGetModel : EntityModel<News>
    {
        public NewsGetModel() : base()
        {
            Entity = new News();
        }

        public NewsGetModel(News news) : base(news)
        {
        }

        [Required(ErrorMessage = nameof(Title) + " is required")]
        public string Title
        {
            get => Entity.Title;
            set => Entity.Title = value;
        }

        [Required(ErrorMessage = nameof(Description) + " is required")]
        public string Description
        {
            get => Entity.Description;
            set => Entity.Description = value;
        }

        [Required(ErrorMessage = nameof(Uri) + " is required")]
        public string Uri
        {
            get => Entity.Uri;
            set => Entity.Uri = value;
        }

        [Required(ErrorMessage = nameof(LastUpdated) + " is required")]
        public DateTimeOffset LastUpdated
        {
            get => Entity.LastUpdated;
            set => Entity.LastUpdated = value;
        }

        [Required(ErrorMessage = nameof(Published) + " is required")]
        public DateTimeOffset Published
        {
            get => Entity.Published;
            set => Entity.Published = value;
        }
    }
}