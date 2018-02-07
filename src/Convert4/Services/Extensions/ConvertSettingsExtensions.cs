using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public static class ConvertSettingsExtensions
    {


        public static ConvertSettings AddFormat<T>(this ConvertSettings settings, string format)
            where T : IFormattable =>
            format == null ? settings : settings?.AddServiceForType<T>("format", format);

        public static string GetFormat<T>(this ConvertSettings settings)
            where T : IFormattable =>
            settings?.GetServiceForType(typeof(T), "format") as string;

        public static string GetFormat(this ConvertSettings settings, Type forType) =>
            settings?.GetServiceForType(forType, "format") as string;
    }
}
