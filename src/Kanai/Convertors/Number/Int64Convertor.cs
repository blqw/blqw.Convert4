using blqw.Kanai;
using blqw.Kanai.Convertors;
using blqw.Kanai.Extensions;
using blqw.Kanai.Froms;
using System;
using System.Globalization;
using static System.Int64;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="long"/> 转换器
    /// </summary>
    public class Int64Convertor : BaseConvertor<long>
                                , IFromConvertible<long>
                                , IFrom<object, long>
                                , IFrom<byte[], long>
    {
        public ConvertResult<long> From(ConvertContext context, bool input) => input ? (long)1 : (long)0;
        public ConvertResult<long> From(ConvertContext context, char input) => (long)input;
        public ConvertResult<long> From(ConvertContext context, sbyte input) => (long)input;
        public ConvertResult<long> From(ConvertContext context, byte input) => (long)input;
        public ConvertResult<long> From(ConvertContext context, short input) => (long)input;
        public ConvertResult<long> From(ConvertContext context, ushort input) => (long)input;
        public ConvertResult<long> From(ConvertContext context, int input) => (long)input;
        public ConvertResult<long> From(ConvertContext context, uint input) => (long)input;
        public ConvertResult<long> From(ConvertContext context, long input) => (long)input;
        public ConvertResult<long> From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context);
            }
            return (long)input;
        }
        public ConvertResult<long> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (long)input;
        }
        public ConvertResult<long> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (long)input;
        }
        public ConvertResult<long> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return decimal.ToInt64(input);
        }
        public ConvertResult<long> From(ConvertContext context, DateTime input) => this.Fail(input, context);
        public ConvertResult<long> From(ConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
            if (TryParse(s, NumberStyles.Any, context.NumberFormatInfo ?? NumberFormatInfo.CurrentInfo, out var result))
            {
                return result;
            }
            if (s.Length > 2)
            {
                if (s[0] == '0')
                {
                    switch (s[1])
                    {
                        case 'x':
                        case 'X':
                            if (TryParse(s.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out result))
                            {
                                return result;
                            }
                            break;
                        case 'b':
                        case 'B':
                            try
                            {
                                return Convert.ToInt64(s.Substring(2), 2);
                            }
                            catch (Exception e)
                            {
                                return this.Error(e, context);
                            }
                        default:
                            break;
                    }
                }
                else if (s[0] == '&' && (s[1] == 'H' || s[1] == 'h'))
                {
                    if (TryParse(s.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out result))
                    {
                        return result;
                    }
                }
            }
            return this.Fail(input, context);
        }
        public ConvertResult<long> From(ConvertContext context, object input) => this.Fail(input, context);
        public ConvertResult<long> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(long))
            {
                return this.Fail(input, context);
            }
            return BitConverter.ToInt64(input.Slice(sizeof(long)), 0);
        }
    }
}
