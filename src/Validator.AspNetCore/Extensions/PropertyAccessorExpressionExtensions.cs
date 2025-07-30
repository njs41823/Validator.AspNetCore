using System.Linq.Expressions;
using System.Text;

namespace Validator.AspNetCore.Extensions
{
    internal static class PropertyAccessorExpressionExtensions
    {
        public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> propertyAccessorExpression)
        {
            var memberExpression = GetMemberExpression(propertyAccessorExpression.Body)
                ?? throw new ArgumentException("Expression must be a member expression.", nameof(propertyAccessorExpression));
            
            var pathBuilder = new StringBuilder();

            while (memberExpression is not null)
            {
                if (pathBuilder.Length > 0)
                {
                    pathBuilder.Insert(0, ".");
                }

                pathBuilder.Insert(0, memberExpression.Member.Name);

                memberExpression = GetMemberExpression(memberExpression.Expression);
            }

            return pathBuilder.ToString();
        }

        private static MemberExpression? GetMemberExpression(Expression? expression)
        {
            return expression switch
            {
                MemberExpression memberExpr => memberExpr,
                UnaryExpression unaryExpr when unaryExpr.Operand is MemberExpression operand => operand,
                _ => null
            };
        }
    }
}
