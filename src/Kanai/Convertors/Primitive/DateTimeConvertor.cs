using blqw.Kanai;
using blqw.Kanai.Convertors;
using blqw.Kanai.Extensions;
using blqw.Kanai.Froms;
using System;
using System.Globalization;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="DateTime" /> 转换器
    /// </summary>
    public class DateTimeConvertor : BaseConvertor<DateTime>, IFrom<string, DateTime>, IFrom<IConvertible, DateTime>
    {
        /// <summary>
        /// 日期格式化字符
        /// </summary>
        private static readonly string[] _formats = {
            "HH:mm",
            "HHmmss",
            "HH:mm:ss",
            "yyyyMMdd",
            "yyyy/MM/dd",
            "yyyy-MM-dd",
            "yyyy.MM.dd",
            "yyyyMMddHHmmss",
            "yyyyMMddHHmmssfff",
            "yyyy-MM-dd HH:mm:ss",
            "yyyyMMddHHmmssffffff" ,
            "yyyy-MM-dd HH:mm:ss:fff",
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-dd HH:mm:ss:ffffff" ,
            "yyyy-MM-dd HH:mm:ss.ffffff" , };

        public ConvertResult<DateTime> From(ConvertContext context, IConvertible input)
        {
            if (input?.GetTypeCode() == TypeCode.DateTime)
            {
                return input.ToDateTime(context.FormatProvider);
            }
            return this.Fail(input, context);
        }

        public ConvertResult<DateTime> From(ConvertContext context, string input)
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
            return this.Fail(input, context);
        }
    }
}
