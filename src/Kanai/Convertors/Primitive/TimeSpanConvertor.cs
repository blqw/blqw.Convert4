using blqw.Kanai.Extensions;
using blqw.Kanai.Interface.From;
using System;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// <seealso cref="TimeSpan" /> 转换器
    /// </summary>
    class TimeSpanConvertor : BaseConvertor<TimeSpan>, IFrom<string, TimeSpan>, IFrom<IConvertible, TimeSpan>
    {
        /// <summary>
        /// 日期格式化字符
        /// </summary>
        private static readonly string[] _formats = {
            "hhmmss",
            "hhmmssfff",
            "hhmmssffffff",
            "hh:mm",
            "hh:mm:ss",
            "hh:mm:ss.fff",
            "hh:mm:ss.ffffff",
        };

        public TimeSpanConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ConvertResult<TimeSpan> From(ConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
            if (TimeSpan.TryParse(s, out var result))
            {
                return result;
            }
            foreach (var format in _formats)
            {
                if (s.Length == format.Length)
                {
                    if (TimeSpan.TryParseExact(input, format, null, System.Globalization.TimeSpanStyles.None, out result))
                    {
                        return result;
                    }
                    break;
                }
            }
            return this.Fail(context, input);
        }

        public ConvertResult<TimeSpan> From(ConvertContext context, IConvertible input)
        {
            if (input?.GetTypeCode() == TypeCode.DateTime)
            {
                var time = input.ToDateTime(context.FormatProvider);
                return new TimeSpan(time.Hour, time.Minute, time.Second, time.Millisecond);
            }
            return this.Fail(context, input);
        }
    }
}
