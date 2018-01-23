using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw.Services
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static void ForEach(this IEnumerable enumerable, Action<object> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}
