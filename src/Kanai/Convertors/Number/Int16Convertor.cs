using System.Security.Cryptography;
using System;
using System.Globalization;
using static System.Int16;
using blqw.Kanai;
using blqw.Kanai.Convertors;
using blqw.Kanai.Extensions;
using blqw.Kanai.Froms;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="short"/> 转换器
    /// </summary>
    public class Int16Convertor : BaseConvertor<short>
                                , IFromConvertible<short>
                                , IFrom<object, short>
                                , IFrom<byte[], short>
    {
        public ConvertResult<short> From(ConvertContext context, bool input) => input ? (short)1 : (short)0;
        public ConvertResult<short> From(ConvertContext context, char input) => (short)input;
        public ConvertResult<short> From(ConvertContext context, sbyte input) => (short)input;
        public ConvertResult<short> From(ConvertContext context, byte input) => (short)input;
        public ConvertResult<short> From(ConvertContext context, short input) => (short)input;
        public ConvertResult<short> From(ConvertContext context, ushort input) => (short)input;
        public ConvertResult<short> From(ConvertContext context, int input) => (short)input;
        public ConvertResult<short> From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context.CultureInfo);
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.CultureInfo);
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, ulong input)
        {
            if (input > (int)MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context.CultureInfo);
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.CultureInfo);
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.CultureInfo);
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.CultureInfo);
            }
            return decimal.ToInt16(input);
        }
        public ConvertResult<short> From(ConvertContext context, DateTime input) => this.Fail(input, context.CultureInfo);
        public ConvertResult<short> From(ConvertContext context, string input)
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
                                return Convert.ToInt16(s.Substring(2), 2);
                            }
                            catch (Exception e)
                            {
                                return this.Error(e, context.CultureInfo);
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
            return this.Fail(input, context.CultureInfo);
        }
        public ConvertResult<short> From(ConvertContext context, object input) => this.Fail(input, context.CultureInfo);
        public ConvertResult<short> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(short))
            {
                return this.Fail(input, context.CultureInfo);
            }
            return BitConverter.ToInt16(input.Slice(sizeof(short)), 0);
        }
    }
}