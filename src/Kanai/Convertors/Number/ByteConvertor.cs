using blqw.Kanai;
using blqw.Kanai.Convertors;
using blqw.Kanai.Extensions;
using blqw.Kanai.Froms;
using System;
using System.Globalization;
using static System.Byte;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="byte" /> 转换器
    /// </summary>
    public class ByteConvertor : BaseConvertor<byte>, IFromConvertible<byte>
    {
        public ConvertResult<byte> From(ConvertContext context, bool input) => input ? (byte)1 : (byte)0;
        public ConvertResult<byte> From(ConvertContext context, char input) => (byte)input;
        public ConvertResult<byte> From(ConvertContext context, sbyte input)
        {
            if (input < MinValue)
            {
                return this.Overflow($"{input} < {MinValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, byte input) => input;
        public ConvertResult<byte> From(ConvertContext context, short input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, ushort input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, int input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, uint input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, long input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, ulong input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, float input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, double input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, decimal input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(ConvertContext context, DateTime input)
        {
            return this.Fail(input, context);
        }
        public ConvertResult<byte> From(ConvertContext context, string input)
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
                                return Convert.ToByte(s.Substring(2), 2);
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
    }
}
