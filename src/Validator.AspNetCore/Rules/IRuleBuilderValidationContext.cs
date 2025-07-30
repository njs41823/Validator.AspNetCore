namespace Validator.AspNetCore.Rules
{
    internal interface IRuleBuilderValidationContext<T, out TProperty>
    {
        T? Instance { get; }

        TProperty? PropertyValue { get; }

        string PropertyName { get; }
    }
}
