namespace Validator.AspNetCore.Rules
{
    public static partial class ComparableRuleBuilderExtensions
    {
        public static IRuleBuilder<T, TProperty> LessThan<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty lessThanValue)
            where TProperty : IComparable<TProperty>
        {
            ruleBuilder.AddRule(validationContext =>
            {
                return validationContext.PropertyValue is not null && validationContext.PropertyValue.CompareTo(lessThanValue) >= 0
                    ? new ValidationFailure(validationContext.PropertyName, "LessThanRule", $"{validationContext.PropertyName} must be less than {lessThanValue}.")
                    : true;
            });

            return ruleBuilder;
        }

        public static IRuleBuilder<T, TProperty> GreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty greaterThanValue)
            where TProperty : IComparable<TProperty>
        {
            ruleBuilder.AddRule(validationContext =>
            {
                return validationContext.PropertyValue is not null && validationContext.PropertyValue.CompareTo(greaterThanValue) <= 0
                    ? new ValidationFailure(validationContext.PropertyName, "GreaterThanRule", $"{validationContext.PropertyName} must be greater than {greaterThanValue}.")
                    : true;
            });

            return ruleBuilder;
        }

        public static IRuleBuilder<T, TProperty> LessThanOrEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty lessThanOrEqualToValue)
            where TProperty : IComparable<TProperty>
        {
            ruleBuilder.AddRule(validationContext =>
            {
                return validationContext.PropertyValue is not null && validationContext.PropertyValue.CompareTo(lessThanOrEqualToValue) > 0
                    ? new ValidationFailure(validationContext.PropertyName, "LessThanOrEqualToRule", $"{validationContext.PropertyName} must be less than or equal to {lessThanOrEqualToValue}.")
                    : true;
            });

            return ruleBuilder;
        }

        public static IRuleBuilder<T, TProperty> GreaterThanOrEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty greaterThanOrEqualToValue)
            where TProperty : IComparable<TProperty>
        {
            ruleBuilder.AddRule(validationContext =>
            {
                return validationContext.PropertyValue is not null && validationContext.PropertyValue.CompareTo(greaterThanOrEqualToValue) < 0
                    ? new ValidationFailure(validationContext.PropertyName, "GreaterThanOrEqualToRule", $"{validationContext.PropertyName} must be greater than or equal to {greaterThanOrEqualToValue}.")
                    : true;
            });

            return ruleBuilder;
        }
    }
}
