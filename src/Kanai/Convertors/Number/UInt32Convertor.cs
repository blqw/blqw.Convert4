using blqw.Kanai;
using blqw.Kanai.Convertors;
using blqw.Kanai.Extensions;
using blqw.Kanai.Froms;
using System;
using System.Globalization;
using static System.UInt32;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="uint"/> 转换器
    /// </summary>
    public class UInt32Convertor : BaseConvertor<uint>
                                , IFromConvertible<uint>
                                , IFrom<object, uint>
                                , IFrom<byte[], uint>
    {
        public ConvertResult<uint> From(ConvertContext context, bool input) => input ? (uint)1 : (uint)0;
        public ConvertResult<uint> From(ConvertContext context, char input) => (uint)input;
        public ConvertResult<uint> From(ConvertContext context, sbyte input) => (uint)input;
        public ConvertResult<uint> From(ConvertContext context, byte input) => (uint)input;
        public ConvertResult<uint> From(ConvertContext context, short input) => (uint)input;
        public ConvertResult<uint> From(ConvertContext context, ushort input) => (uint)input;
        public ConvertResult<uint> From(ConvertContext context, int input) => (uint)input;
        public ConvertResult<uint> From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return decimal.ToUInt32(input);
        }
        public ConvertResult<uint> From(ConvertContext context, DateTime input) => this.Fail(input, context);
        public ConvertResult<uint> From(ConvertContext context, string input)
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
                                return Convert.ToUInt32(s.Substring(2), 2);
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
        public ConvertResult<uint> From(ConvertContext context, object input) => this.Fail(input, context);
        public ConvertResult<uint> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(uint))
            {
                return this.Fail(input, context);
            }
            return BitConverter.ToUInt32(input.Slice(sizeof(uint)), 0);
        }
    }
}
