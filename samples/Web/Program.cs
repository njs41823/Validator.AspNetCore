using Microsoft.AspNetCore.Mvc;
using Validator.AspNetCore;
using Validator.AspNetCore.DependencyInjection;
using Web.Contracts;
using Web.Services;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IForbiddenIdAccessor, ForbiddenIdAccessor>();

            // add all concrete IValidator<> implementations to the service collection
            builder.Services.AddValidatorsFromAssemblies(ServiceLifetime.Scoped, AssemblyReference.Assembly);

            var app = builder.Build();

            app.MapPost("/test", (
                [FromBody] Person testRequest,
                // use DI to inject IValidator<Person> implementations
                [FromServices] IEnumerable<IValidator<Person>> validators) =>
            {
                return ValidationResult.Combine(validators.Select(x => x.Validate(testRequest)));
            });

            app.Run();
        }
    }
}
