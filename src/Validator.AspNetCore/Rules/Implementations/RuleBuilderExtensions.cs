using System.Collections;
using Validator.AspNetCore.Extensions;

namespace Validator.AspNetCore.Rules
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, TProperty> NotNull<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            ruleBuilder.AddRule(validationContext =>
            {
                return validationContext.PropertyValue is null
                    ? new ValidationFailure(validationContext.PropertyName, "NotNullRule", $"{validationContext.PropertyName} cannot be null.")
                    : true;
            });

            return ruleBuilder;
        }

        public static IRuleBuilder<T, TProperty> NotEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            ruleBuilder.AddRule(validationContext =>
            {
                var validationFailure = new ValidationFailure(validationContext.PropertyName, "NotEmptyRule", $"{validationContext.PropertyName} cannot be empty.");

                if (validationContext.PropertyValue is null)
                {
                    return validationFailure;
                }

                if (validationContext.PropertyValue is string s && string.IsNullOrWhiteSpace(s))
                {
                    return validationFailure;
                }

                if (validationContext.PropertyValue is ICollection col && col.Count == 0)
                {
                    return validationFailure;
                }

                if (validationContext.PropertyValue is IEnumerable e && e.IsEmpty())
                {
                    return validationFailure;
                }

                if (EqualityComparer<TProperty>.Default.Equals(validationContext.PropertyValue, default))
                {
                    return validationFailure;
                }

                return true;
            });

            return ruleBuilder;
        }

        public static IRuleBuilder<T, TProperty> MinimumLength<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, int minimumLength)
        {
            ruleBuilder.AddRule(validationContext =>
            {
                var validationFailure = new ValidationFailure(validationContext.PropertyName, "MinimumLengthRule", $"{validationContext.PropertyName} does not meet the minimum length of {minimumLength}.");

                if (validationContext.PropertyValue is null)
                {
                    return true;
                }

                if (validationContext.PropertyValue is string s && s?.Length < minimumLength)
                {
                    return validationFailure;
                }

                if (validationContext.PropertyValue is ICollection col && col.Count < minimumLength)
                {
                    return validationFailure;
                }

                if (validationContext.PropertyValue is IEnumerable e && e.Count() < minimumLength)
                {
                    return validationFailure;
                }

                return true;
            });

            return ruleBuilder;
        }

        public static IRuleBuilder<T, TProperty> MaximumLength<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, int maximumLength)
        {
            ruleBuilder.AddRule(validationContext =>
            {
                var validationFailure = new ValidationFailure(validationContext.PropertyName, "MaximumLengthRule", $"{validationContext.PropertyName} exceeds the maximum length of {maximumLength}.");

                if (validationContext.PropertyValue is null)
                {
                    return true;
                }

                if (validationContext.PropertyValue is string s && s?.Length > maximumLength)
                {
                    return validationFailure;
                }

                if (validationContext.PropertyValue is ICollection col && col.Count > maximumLength)
                {
                    return validationFailure;
                }

                if (validationContext.PropertyValue is IEnumerable e && e.Count() > maximumLength)
                {
                    return validationFailure;
                }

                return true;
            });

            return ruleBuilder;
        }
    }
}
