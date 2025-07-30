using System.Linq.Expressions;
using Validator.AspNetCore.Rules;

namespace Validator.AspNetCore
{
    public abstract class AbstractValidator<T> : IValidator<T>
    {
        private readonly List<RuleBuilder<T>> ruleBuilders = [];

        public IRuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertyAccessorExpression)
        {
            var ruleBuilder = new RuleBuilder<T, TProperty>(new(propertyAccessorExpression));

            ruleBuilders.Add(ruleBuilder);

            return ruleBuilder;
        }

        public ValidationResult Validate(T? instance)
        {
            return ValidationResult.Combine(ruleBuilders.Select(x => x.Validate(instance)));
        }
    }
}
