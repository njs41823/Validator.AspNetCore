namespace Validator.AspNetCore.Extensions
{
    internal static class TypeExtensions
    {
        public static Type? GetClosedValidatorInterface(this Type type)
        {
            if (type.IsInterface || type.IsAbstract || type.IsGenericType)
            {
                return null;
            }

            return type
                .GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidator<>));
        }
    }
}
