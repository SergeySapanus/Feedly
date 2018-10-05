using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MyFeedlyServer.Extensions
{
    public static class ControllerExtensions
    {
        public static bool IsSameUser(this Controller controller, int userId)
        {
            var value = controller.User.Claims.Where(c => c.Type == nameof(Entities.User.Id)).Select(c => c.Value).FirstOrDefault();
            return !string.IsNullOrWhiteSpace(value) && int.TryParse(value, out var id) && userId == id;
        }
    }
}