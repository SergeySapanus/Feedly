using MyFeedlyServer.Entities.Contracts;
using MyFeedlyServer.Entities.Models;

namespace MyFeedlyServer.Entities.Extensions
{
    public static class EntityModelExtensions
    {
        public static bool IsNull<T>(this EntityModel<T> model) where T : IEntity
        {
            return ReferenceEquals(model, null) || model.IsNullEntity();
        }
    }
}