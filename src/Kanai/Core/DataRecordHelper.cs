using System;
using System.Data;

namespace blqw.Kanai.Core
{
    static class DataRecordHelper
    {
        public static ConvertResult<T> Convert<T>(this IDataRecord record, int index)
        {
            var handler = Getter<T>.GetValue;
            if (handler != null)
            {
                return handler(record, index);
            }
            return new ConvertResult<T>(false, default, null);
        }

        class Getter<T>
        {
            public static readonly Func<IDataRecord, int, T> GetValue = CreateHandler();

            private static Func<IDataRecord, int, T> CreateHandler()
            {
                if (typeof(T).IsMetaType())
                {
                    var name = "Get" + typeof(T).GetFriendlyName();
                    var method = typeof(T).GetMethod(name);
                    if (method != null)
                    {
                        return (Func<IDataRecord, int, T>)method.CreateDelegate(typeof(Func<IDataRecord, int, T>));
                    }
                }
                return null;
            }
        }

    }
}
