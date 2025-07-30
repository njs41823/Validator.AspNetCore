# Validator.AspNetCore
   
Simple, fluent interface validation implementation for ASP.NET Core.

## Installing Validator.AspNetCore

You should install Mediator.CodeGen with NuGet or the .NET Core command line interface:

`Install-Package Validator.AspNetCore`

or

`dotnet add package Validator.AspNetCore`

## Getting Started
Validator.AspNetCore supports Microsoft.Extensions.DependencyInjection.Abstractions directly:

`services.AddValidatorsFromAssemblies(assembly1, assembly2);`

(this registers concrete IValidator<> implementations as singletons);

OR

`services.AddValidatorsFromAssemblies(ServiceLifetime.Scoped, assembly1, assembly2);`

(this overload registers concrete IValidator<> implementations with the desired ServiceLifetime);

## Usage (see /samples directory for more)

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

## Custom Rules

```
using Validator.AspNetCore;
using Validator.AspNetCore.Rules;

public static class RuleBuilderExtensions
{
   private const int MaximumIdValue = int.MaxValue - 69;

   // add custom rules by extending the IRuleBuilder<,> interface
   public static IRuleBuilder<T, int> RejectHugeIds<T>(this IRuleBuilder<T, int> ruleBuilder)
   {
      ruleBuilder.AddRule(validationContext =>
      {
         if (validationContext.PropertyValue > MaximumIdValue)
         {
            return new ValidationFailure(
               PropertyName: validationContext.PropertyName,
               ErrorCode: "RejectHugeIdsRule",
               ErrorMessage: $"{validationContext.PropertyName} cannot be greater than {MaximumIdValue}.");
         }

         return true;
      });

      return ruleBuilder;
   }
}
```
