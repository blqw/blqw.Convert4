using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace blqw
{
    static class DisposeHelper
    {
        public static void DisposeAndRemove<T>(ref T obj) where T : class =>
            Dispose(Interlocked.Exchange(ref obj, null));


        public static void Dispose(object obj)
        {
            if (obj is IDisposable disposable)
            {
                try
                {
                    disposable.Dispose();
                }
                catch
                {

                }
            }
            else if (obj is IEnumerable enumerable)
            {
                enumerable.Cast<object>().ToList().ForEach(Dispose);
            }
        }
    }
}
