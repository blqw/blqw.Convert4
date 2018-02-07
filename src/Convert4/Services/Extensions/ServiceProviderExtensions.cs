using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace blqw.Services
{
    /// <summary>
    /// 上下文扩展方法
    /// </summary>
    static class ServiceProviderExtensions
    {
        public static CultureInfo GetCultureInfo(this IServiceProvider context) =>
            context?.GetService(typeof(CultureInfo)) as CultureInfo ?? CultureInfo.CurrentCulture;

        public static IFormatProvider GetFormatProvider(this IServiceProvider context, Type forType) =>
            context?.GetService<ConvertSettings>()?.GetServiceForType(forType, typeof(IFormatProvider)) as IFormatProvider
            ?? context?.GetService<IFormatProvider>();

        public static Encoding GetEncoding(this IServiceProvider context) =>
                   context?.GetService(typeof(Encoding)) as Encoding;

        public static StreamingContextStates GetStreamingContextStates(this IServiceProvider context) =>
            context?.GetService(typeof(StreamingContextStates)) is StreamingContextStates v ? v : StreamingContextStates.All;

        public static StringSplitOptions GetStringSplitOptions(this IServiceProvider context) =>
            context?.GetService(typeof(StringSplitOptions)) is StringSplitOptions v ? v : StringSplitOptions.None;

        public static T Get<T>(this IServiceProvider context, string name, T defaultValue = default)
            => (context?.GetService(typeof(ConvertSettings)) as ConvertSettings)?.GetService("StringSeparator") is T t ? t : defaultValue;

        public static T Get<T>(this IServiceProvider context, T defaultValue = default)
            => context?.GetService(typeof(T)) is T t ? t : defaultValue;

        public static object Get(this IServiceProvider context, string name)
            => (context?.GetService(typeof(ConvertSettings)) as ConvertSettings)?.GetService("StringSeparator");

        public static object GetStringSeparators(this IServiceProvider context)
        {
            var separator = context.Get("StringSeparator");
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

        public static string GetStringSeparator(this IServiceProvider context)
        {
            var separator = context.Get("StringSeparator");
            switch (separator)
            {
                case char[] r:
                    return r.Length > 0 ? r.ToString() : null;
                case char r:
                    return r.ToString();
                case string r:
                    return r;
                case string[] r:
                    return r.Length > 0 ? r[0] : null;
                default:
                    return null;
            }
        }

        public static SerializationService GetSerialization(this IServiceProvider context)
        {
            var contract = context.Get<SerializationContract>();
            if (contract == 0)
            {
                return null;
            }
            return context.Get<SerializationService>(contract + "Serialization");
        }

        public static string GetFormat(this ConvertContext context, object input) =>
            input == null ? null : context.GetService<ConvertSettings>().GetFormat(input.GetType());


    }
}
