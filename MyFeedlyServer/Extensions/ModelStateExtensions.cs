using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace MyFeedlyServer.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetAllErrors(this ModelStateDictionary modelState)
        {
            return string.Join("; ", modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
        }
    }
}
