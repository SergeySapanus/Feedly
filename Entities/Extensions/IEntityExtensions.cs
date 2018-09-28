using Entities.Abstracts;

namespace Entities.Extensions
{
    public static class IEntityExtensions
    {
        public static bool IsNull(this IEntity entity)
        {
            return ReferenceEquals(entity, null);
        }

        public static bool IsNewObject(this IEntity entity)
        {
            return entity.Id.Equals(0);
        }
    }
}
