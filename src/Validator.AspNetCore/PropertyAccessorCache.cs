using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Validator.AspNetCore
{
    internal static class PropertyAccessorCache<T>
    {
        private static readonly ConcurrentDictionary<LambdaExpression, Delegate> cache = new(new ExpressionEqualityComparer());

        public static Func<T, TProperty> Get<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            return (Func<T, TProperty>)cache.GetOrAdd(expression, valueFactory =>
            {
                return expression.Compile();
            });
        }
    }

    internal sealed class ExpressionEqualityComparer : IEqualityComparer<LambdaExpression?>
    {
        public bool Equals(LambdaExpression? x, LambdaExpression? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return ExpressionComparer.AreEqual(x, y);
        }

        public int GetHashCode(LambdaExpression? obj)
        {
            return obj is null
                ? 0
                : ExpressionHasher.GetHashCode(obj);
        }
    }

    internal static class ExpressionComparer
    {
        public static bool AreEqual(Expression x, Expression y)
        {
            return ExpressionComparisonVisitor.Compare(x, y);
        }

        private class ExpressionComparisonVisitor
        {
            public static bool Compare(Expression? x, Expression? y)
            {
                if (x is null || y is null)
                {
                    return false;
                }

                if (x == y)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                if (x.NodeType != y.NodeType || x.Type != y.Type)
                {
                    return false;
                }

                return x switch
                {
                    MemberExpression mx when y is MemberExpression my => mx.Member == my.Member && Compare(mx?.Expression, my?.Expression),
                    ParameterExpression px when y is ParameterExpression py => px.Name == py.Name,
                    LambdaExpression lx when y is LambdaExpression ly => Compare(lx.Body, ly.Body),
                    _ => x.ToString() == y.ToString()
                };
            }
        }
    }

    internal static class ExpressionHasher
    {
        public static int GetHashCode(Expression? expression)
        {
            return new HashVisitor().ComputeHash(expression);
        }

        private class HashVisitor : ExpressionVisitor
        {
            private int hash;

            public int ComputeHash(Expression? expression)
            {
                this.hash = 17;

                this.Visit(expression);
                
                return this.hash;
            }

            public override Expression? Visit(Expression? node)
            {
                if (node is null)
                {
                    this.hash = this.hash * 23 + 0;

                    return null;
                }

                this.hash = this.hash * 23 + (int)node.NodeType;

                this.hash = this.hash * 23 + node.Type.GetHashCode();

                return base.Visit(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                this.hash = this.hash * 23 + node.Member.GetHashCode();

                this.Visit(node.Expression);

                return node;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                this.hash = this.hash * 23 + (node.Name?.GetHashCode() ?? 0);

                return node;
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                this.Visit(node.Body);
                
                foreach (var param in node.Parameters)
                {
                    this.Visit(param);
                }

                return node;
            }

            // Add more overrides as needed
        }
    }
}
