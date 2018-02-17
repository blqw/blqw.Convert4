using blqw.ConvertServices;
using System;
using System.Globalization;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="DateTimeConvertor" /> 转换器
    /// </summary>
    public class DateTimeConvertor : BaseConvertor<DateTime>, IFrom<DateTime, string>, IFrom<DateTime, IConvertible>
    {
        /// <summary>
        /// 日期格式化字符
        /// </summary>
        private static readonly string[] _formats = { "yyyyMMdd", "HHmmss", "yyyyMMddHHmmss", "yyyyMMddHHmmssfff", "yyyyMMddHHmmssffffff" };

        public DateTime From(ConvertContext context, IConvertible input)
        {
            if (input?.GetTypeCode() == TypeCode.DateTime)
            {
                return input.ToDateTime(context.GetFormatProvider(typeof(DateTime)));
            }
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }

        public DateTime From(ConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
            if (DateTime.TryParse(s, out var result))
            {
                return result;
            }
            foreach (var format in _formats)
            {
                if (s.Length == format.Length)
                {
                    if (DateTime.TryParseExact(input, format, null, DateTimeStyles.None, out result))
                    {
                        return result;
                    }
                    break;
                }
            }
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
    }
}