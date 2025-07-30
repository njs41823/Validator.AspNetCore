using System.Collections;
using System.Text;

namespace Validator.AspNetCore.Extensions
{
    internal static class EnumerableExtensions
    {
        public static bool IsEmpty(this IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();

            using (enumerator as IDisposable)
            {
                return !enumerator.MoveNext();
            }
        }

        public static int Count(this IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();

            var result = 0;

            using (enumerator as IDisposable)
            {
                while (enumerator.MoveNext())
                {
                    result++;
                }
            }

            return result;
        }
    }
}
