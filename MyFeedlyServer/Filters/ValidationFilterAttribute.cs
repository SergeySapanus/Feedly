using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyFeedlyServer.Contracts;
using MyFeedlyServer.Controllers;
using MyFeedlyServer.Extensions;
using MyFeedlyServer.Resources;

namespace MyFeedlyServer.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly ILoggerManager _logger;

        public ValidationFilterAttribute(ILoggerManager logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                _logger.LogError(string.Format(Resource.LogErrorInvalidModel, context.Controller, context.ModelState.GetAllErrors()));
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            if (context.HttpContext.User.Identity.IsAuthenticated && context.Controller is BaseController controller)
            {
                var autorizedUserId = controller.GetAuthorizedUserId();
                if (!autorizedUserId.HasValue)
                {
                    _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                    context.Result = new UnauthorizedResult();
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}