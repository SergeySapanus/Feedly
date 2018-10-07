using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MyFeedlyServer.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetUserIdTypeName(this Controller controller) => "UserId";

        public static int? GetAutorizedUserId(this Controller controller)
        {
            var value = controller.User.Claims.Where(c => c.Type == GetUserIdTypeName(controller)).Select(c => c.Value).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(value) || !int.TryParse(value, out var userId))
                return null;

            return userId;
        }
    }
}