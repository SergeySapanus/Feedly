using MyFeedlyServer.Entities.Contracts;

namespace MyFeedlyServer.Models.Extensions
{
    public static class EntityModelExtensions
    {
        public static bool IsNull<T>(this EntityModel<T> model) where T : IEntity
        {
            return ReferenceEquals(model, null) || model.IsNullEntity();
        }
    }
}