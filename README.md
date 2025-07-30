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
