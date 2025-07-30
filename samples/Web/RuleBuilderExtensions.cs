using Validator.AspNetCore;
using Validator.AspNetCore.Rules;

namespace Web
{
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
}
