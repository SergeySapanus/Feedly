using System.Text;
using Entities.Contracts;

namespace Entities.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsNull(this IEntity entity)
        {
            return ReferenceEquals(entity, null);
        }

        public static int GetUriHashCode(this IUriEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Uri))
                return 0;

            var bytes = Encoding.Unicode.GetBytes(entity.Uri);
            var sum = 0;

            foreach (var @byte in bytes)
                for (var i = 0; i < 8; i++)
                    sum += (@byte >> i) & 1;

            return sum;
        }

        public static bool UriEquals(this IUriEntity entity, object obj)
        {
            if (!(obj is IUriEntity uriEntity))
                return false;

            if (ReferenceEquals(entity, uriEntity))
                return true;

            if (entity.Uri.Length != uriEntity.Uri.Length)
                return false;

            return entity.Uri == uriEntity.Uri;
        }
    }
}
