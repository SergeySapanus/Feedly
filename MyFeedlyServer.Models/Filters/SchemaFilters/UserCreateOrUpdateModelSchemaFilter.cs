using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyFeedlyServer.Models.Filters.SchemaFilters
{
    public class UserCreateOrUpdateModelSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            schema.Example = new UserCreateOrUpdateModel
            {
                Name = nameof(UserCreateOrUpdateModel.Name),
                Password = nameof(UserCreateOrUpdateModel.Password)
            };
        }
    }
}