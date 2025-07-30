using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Validator.AspNetCore.Extensions;

namespace Validator.AspNetCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValidatorsFromAssemblies(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            return services.AddValidatorsFromAssemblies(ServiceLifetime.Singleton, assemblies);
        }

        public static IServiceCollection AddValidatorsFromAssemblies(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            params Assembly[] assemblies)
        {
            var closedValidatorTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                    !x.IsInterface &&
                    !x.IsAbstract &&
                    x
                        .GetInterfaces()
                        .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidator<>)));

            foreach (var closedValidatorType in closedValidatorTypes)
            {
                var closedValidatorInterface = closedValidatorType.GetClosedValidatorInterface();

                if (closedValidatorInterface is null)
                {
                    continue;
                }

                services.Add(new ServiceDescriptor(closedValidatorInterface, closedValidatorType, serviceLifetime));
            }

            return services;
        }
    }
}
