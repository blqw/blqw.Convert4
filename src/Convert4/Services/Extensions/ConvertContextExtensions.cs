using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace blqw.Services
{
    /// <summary>
    /// 上下文扩展方法
    /// </summary>
    public static class ConvertContextExtensions
    {
        public static CultureInfo GetCultureInfo(this IServiceProvider context) =>
            context?.GetService(typeof(CultureInfo)) as CultureInfo ?? CultureInfo.CurrentCulture;

        public static StreamingContextStates GetStreamingContextStates(this IServiceProvider context) =>
            context?.GetService(typeof(StreamingContextStates)) is StreamingContextStates v ? v : StreamingContextStates.All;

    }
}
