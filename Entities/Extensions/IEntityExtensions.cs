using Entities.Abstract;

namespace Entities.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsNull(this IEntity entity)
        {
            return ReferenceEquals(entity, null);
        }
    }
}
