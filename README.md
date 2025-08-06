# Validator.AspNetCore

Simple, fluent interface validation implementation for ASP.NET Core.

#### Installing Validator.AspNetCore
You should install Mediator.CodeGen with NuGet or the .NET Core command line interface:

`Install-Package Validator.AspNetCore`

or

`dotnet add package Validator.AspNetCore`

#### Getting Started
Validator.AspNetCore supports Microsoft.Extensions.DependencyInjection.Abstractions directly:

`services.AddValidator(assembly1, assembly2);`

(this registers an IValidator, and any concrete IValidator<> implementations, as singletons);

OR

`services.AddValidator(ServiceLifetime.Scoped, assembly1, assembly2);`

(this registers an IValidator, and any concrete IValidator<> implementations, with the desired ServiceLifetime);

#### Usage (see /samples directory for more)
```
using Validator.AspNetCore;

public sealed class PersonValidator : AbstractValidator<Person>
{
   public PersonValidator()
   {
      this
         .RuleFor(x => x.Id)
         .NotEmpty()
         .GreaterThanOrEqualTo(0);

      this
         .RuleFor(x => x.FirstName)
         .NotEmpty()
         .MaximumLength(100);
   }
}
```
#### Custom Rules
```
using Validator.AspNetCore;
using Validator.AspNetCore.Rules;

public static class RuleBuilderExtensions
{
    private const int FunnyIdValue = 69;

    // add custom rules by extending the IRuleBuilder<,> interface
    public static IRuleBuilder<T, int> RejectFunnyId<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        ruleBuilder.AddRule((propertyValue, validationContext) =>
        {
            if (propertyValue != FunnyIdValue)
            {
                return;
            }

            validationContext.AddFailure(new ValidationFailure(
               PropertyName: validationContext.PropertyName,
               ErrorCode: "RejectFunnyIdRule",
               ErrorMessage: $"{validationContext.PropertyName} cannot be equal to {FunnyIdValue}."));
        });

        return ruleBuilder;
    }
}
```
#### Validation
```
// Inject IValidator to manually validate
app
    .MapPost("/user", async (
        [FromBody] CreateUserRequest request,
        [FromServices] IValidator validator,
        CancellationToken cancellationToken) =>
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        ...
    });

// Use AddValidation() to add an IEndpointFilter that validates the parameters automatically
app
    .MapPost("/user", async (
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken) =>
    {
        ...
    })
    .AddValidation(options =>
    {
        // specify how to generate a response when validation fails
        options.OnFailure = validationFailures => new { isSuccess = false, error = validationFailures.FirstOrDefault()?.ErrorCode }
    });
```
