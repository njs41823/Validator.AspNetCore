namespace Validator.AspNetCore.Rules
{
    public interface IRuleBuilder<T>
    {
        ValidationResult Validate(T instance, string? propertyPrefix = null);
    }

    public interface IRuleBuilder<T, TProperty> : IRuleBuilder<T>
    {
        IRuleBuilder<T, TProperty> AddRule(Func<RuleBuilderValidationContext<T, TProperty>, ValidationResult> rule);

        IRuleBuilder<T, TProperty> SetValidator(IValidator<TProperty> validator);
    }

    internal abstract class RuleBuilder<T> : IRuleBuilder<T>
    {
        public abstract ValidationResult Validate(T? instance, string? propertyPrefix = null);
    }

    internal sealed class RuleBuilder<T, TProperty>(PropertyAccessor<T, TProperty> propertyAccessor) : RuleBuilder<T>, IRuleBuilder<T, TProperty>
    {
        private readonly PropertyAccessor<T, TProperty> propertyAccessor = propertyAccessor;

        private readonly List<Func<T?, PropertyAccessor<T, TProperty>, ValidationResult>> rules = [];

        public IRuleBuilder<T, TProperty> AddRule(Func<RuleBuilderValidationContext<T, TProperty>, ValidationResult> rule)
        {
            this.rules.Add((instance, propertyAccessor) =>
            {
                return rule(new RuleBuilderValidationContext<T, TProperty>(instance, propertyAccessor.GetPropertyValue(instance), propertyAccessor.PropertyName));
            });

            return this;
        }

        public IRuleBuilder<T, TProperty> SetValidator(IValidator<TProperty> validator)
        {
            this.rules.Add((instance, propertyAccessor) =>
            {
                var nestedResult = validator.Validate(propertyAccessor.GetPropertyValue(instance));

                var validationFailures = nestedResult.Failures
                    .Select(x => new ValidationFailure(
                        PropertyName: $"{propertyAccessor.PropertyName}.{x.PropertyName}",
                        ErrorCode: x.ErrorCode,
                        ErrorMessage: x.ErrorMessage));

                return new ValidationResult([.. validationFailures]);
            });

            return this;
        }

        public override ValidationResult Validate(T? instance, string? propertyPrefix = null)
        {
            return ValidationResult.Combine(rules
                .Select(rule => rule(instance, propertyAccessor)));
        }
    }
}
