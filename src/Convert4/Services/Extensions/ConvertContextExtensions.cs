﻿using System;
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

        public static StringSplitOptions GetStringSplitOptions(this IServiceProvider context) =>
            context?.GetService(typeof(StringSplitOptions)) is StringSplitOptions v ? v : StringSplitOptions.None;

        public static object GetStringSeparator(this IServiceProvider context)
        {
            var service = (NamedService)context?.GetService(typeof(NamedService));
            if (service == null)
            {
                return null;
            }
            var separator = service["StringSeparator"];
            switch (separator)
            {
                case char[] r:
                    return r;
                case char r:
                    return new char[] { r };
                case string r:
                    return new string[] { r };
                case string[] r:
                    return r;
                default:
                    return null;
            }
        }
    }
}
