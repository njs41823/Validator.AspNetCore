namespace Validator.AspNetCore.Rules
{

    public sealed class RuleBuilderValidationContext<T, TProperty>(
        T? instance,
        TProperty? propertyValue,
        string propertyName) : IRuleBuilderValidationContext<T, TProperty>
    {
        public T? Instance { get; } = instance;

        public TProperty? PropertyValue { get; } = propertyValue;

        public string PropertyName { get; } = propertyName;
    }
}
