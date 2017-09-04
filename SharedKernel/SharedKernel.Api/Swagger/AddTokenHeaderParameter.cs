using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace SharedKernel.Api.Swagger
{
    public class AddTokenHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter
            {
                name = "Token",
                @in = "header",
                type = "string",
                required = false
            });
        }
    }
}
