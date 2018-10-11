using System.Security.Cryptography;
using System;
using System.Globalization;
using static System.Int16;
using blqw.ConvertServices;

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
                return context.OverflowException($"{input} > {MaxValue}");
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, ulong input)
        {
            if (input > (int)MaxValue)
            {
                return context.OverflowException($"{input} > {MaxValue}");
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (short)input;
        }
        public ConvertResult<short> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return decimal.ToInt16(input);
        }
        public ConvertResult<short> From(ConvertContext context, DateTime input) => context.InvalidCastException(input, TypeFriendlyName);
        public ConvertResult<short> From(ConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(s))
            {
                return 0;
            }
            if (TryParse(s, out var result))
            {
                return result;
            }
            var hex = s.ToHex();
            if (hex != null && TryParse(hex, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out result))
            {
                return result;
            }
            return context.InvalidCastException(input, TypeFriendlyName);
        }
        public ConvertResult<short> From(ConvertContext context, object input) => context.InvalidCastException(input, TypeFriendlyName);
        public ConvertResult<short> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(short))
            {
                return context.InvalidCastException(input, TypeFriendlyName);
            }
            return BitConverter.ToInt16(input.Slice(sizeof(short)), 0);
        }
    }
}