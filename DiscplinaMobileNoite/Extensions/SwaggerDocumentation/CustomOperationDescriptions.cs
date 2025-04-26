using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiscplinaMobileNoite.Extensions.SwaggerDocumentation
{
    public class CustomOperationDescriptions : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context?.ApiDescription?.HttpMethod is null || context.ApiDescription.RelativePath is null)
                return;

            var routeHandlers = new Dictionary<string, Action>
                {
                    { "user", () => HandleUserOperations(operation, context) },
                    { "points", () => HandlePointOperations(operation, context) },
                    { "justification", () => HandleJustificationOperations(operation, context) },
                    { "recoverPassword", () => HandleRecoverPasswordOperations(operation, context) }
                };

            foreach (var routeHandler in routeHandlers)
            {
                if (context.ApiDescription.RelativePath.Contains(routeHandler.Key))
                {
                    routeHandler.Value.Invoke();
                    break;
                }
            }
        }

        private void HandleUserOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Create a new User";
                operation.Description = "This endpoint allows you to create a new User by providing the necessary details.";
                AddResponses(operation, "200", "The User was successfully created.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Update an existing User";
                operation.Description = "This endpoint allows you to update an existing User by providing the necessary details.";
                AddResponses(operation, "200", "The User was successfully updated.");
            }
            else if (method == "DELETE")
            {
                operation.Summary = "Delete an existing User";
                operation.Description = "This endpoint allows you to delete an existing User by providing the ID.";
                AddResponses(operation, "200", "The User was successfully deleted.");
                AddResponses(operation, "404", "User not found. Please verify the ID.");
            }
            else if (method == "GET")
            {
                if (path.Contains("allactives"))
                {
                    operation.Summary = "Retrieve all active Users";
                    operation.Description = "This endpoint returns all Users where IsActive is true.";
                    AddResponses(operation, "200", "All active Users were successfully retrieved.");
                }
                else if (path.Contains("all"))
                {
                    operation.Summary = "Retrieve all Users";
                    operation.Description = "This endpoint allows you to retrieve details of all existing Users.";
                    AddResponses(operation, "200", "All User details were successfully retrieved.");
                }
            }
        }

        private void HandlePointOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Create a new point";
                operation.Description = "This endpoint allows you to create a new point.";
                AddResponses(operation, "200", "The point was successfully created.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Update an existing point";
                operation.Description = "This endpoint allows you to update an existing point.";
                AddResponses(operation, "200", "The point was successfully updated.");
            }
            else if (method == "GET")
            {
                if (path.Contains("allactives"))
                {
                    operation.Summary = "Retrieve all active points";
                    operation.Description = "This endpoint returns all poins where IsActive is true.";
                    AddResponses(operation, "200", "All active attendance Records were successfully retrieved.");
                }
                else if (path.Contains("all"))
                {
                    operation.Summary = "Retrieve all points";
                    operation.Description = "This endpoint allows you to retrieve details of all existing points.";
                    AddResponses(operation, "200", "All points details were successfully retrieved.");
                }
            }
        }

        private void HandleJustificationOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Create a new Justification";
                operation.Description = "This endpoint allows you to create a new justification by providing the necessary details.";
                AddResponses(operation, "200", "The justification was successfully created.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Update an existing justification";
                operation.Description = "This endpoint allows you to update an existing attendance Record by providing the necessary details.";
                AddResponses(operation, "200", "The attendance Record was successfully updated.");
            }
            else if (method == "GET")
            {
                if (path.Contains("allactives"))
                {
                    operation.Summary = "Retrieve all active justifications";
                    operation.Description = "This endpoint returns all justifications where IsActive is true.";
                    AddResponses(operation, "200", "All active justifications were successfully retrieved.");
                }
                else if (path.Contains("all"))
                {
                    operation.Summary = "Retrieve all justifications";
                    operation.Description = "This endpoint allows you to retrieve details of all existing justifications.";
                    AddResponses(operation, "200", "All justifications details were successfully retrieved.");
                }
            }
        }

        private void HandleRecoverPasswordOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "PUT")
            {
                operation.Summary = "Update an existing recover password.";
                operation.Description = "This endpoint allows you to update an existing recover password by providing the necessary details.";
                AddResponses(operation, "200", "The recover password was successfully updated.");
            }
        }

        private void AddResponses(OpenApiOperation operation, string statusCode, string description)
        {
            if (!operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses.Add(statusCode, new OpenApiResponse { Description = description });
            }
        }
    }
}
