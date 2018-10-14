using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyFeedlyServer.Filters
{
    public class JWTSecurityRequirementsOperationFilter : IOperationFilter
    {
        private readonly SecurityRequirementsOperationFilter<AuthorizeAttribute> _filter;

        public JWTSecurityRequirementsOperationFilter(bool includeUnauthorizedAndForbiddenResponses = false)
        {
            _filter = new SecurityRequirementsOperationFilter<AuthorizeAttribute>(
                authAttributes => authAttributes.Where(a => !string.IsNullOrEmpty(a.Policy))
                .Select(a => a.Policy), includeUnauthorizedAndForbiddenResponses);
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            _filter.Apply(operation, context);
        }
    }
}