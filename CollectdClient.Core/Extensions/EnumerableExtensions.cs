using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CollectdClient.Core.Extensions
{
    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> elements, Action<T> action)
        {
            foreach (var element in elements)
            {
                action.Invoke(element);
            }
        }
    }
}
