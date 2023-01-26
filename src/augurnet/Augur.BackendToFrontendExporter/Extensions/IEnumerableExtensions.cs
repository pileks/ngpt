using System;
using System.Collections.Generic;

namespace Augur.BackendToFrontendExporter.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TBy>(this IEnumerable<T> enumerable, Func<T, TBy> selectDistinction)
        {
            var dictionary = new Dictionary<TBy, bool>();
            bool defaultCase = false;

            foreach (var item in enumerable)
            {
                if (selectDistinction(item) == null)
                {
                    if (!defaultCase)
                    {
                        defaultCase = true;

                        yield return item;
                    }
                }
                else if (!dictionary.ContainsKey(selectDistinction(item)))
                {
                    dictionary.Add(selectDistinction(item), true);

                    yield return item;
                }
            }
        }
    }
}