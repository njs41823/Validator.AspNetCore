using System.Linq.Expressions;
using Validator.AspNetCore.Extensions;

namespace Validator.AspNetCore
{
    public interface IPropertyAccessor<T, out TProperty>
    {
        string PropertyName { get; }

        TProperty? GetPropertyValue(T? instance);
    }

    internal sealed class PropertyAccessor<T, TProperty>(Expression<Func<T, TProperty>> propertyAccessorExpression) : IPropertyAccessor<T, TProperty>
    {
        private readonly Func<T, TProperty> propertyAccessor = PropertyAccessorCache<T>.Get(propertyAccessorExpression);

        public string PropertyName { get; } = propertyAccessorExpression.GetPropertyName();

        public TProperty? GetPropertyValue(T? instance)
        {
            if (instance is null)
            {
                return default;
            }

            return this.propertyAccessor(instance);
        }
    }
}
