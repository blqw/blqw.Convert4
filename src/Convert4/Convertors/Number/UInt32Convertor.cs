using blqw.ConvertServices;
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
                return context.OverflowException($"{input} > {MaxValue}");
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return context.OverflowException($"{input} > {MaxValue}");
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return decimal.ToUInt32(input);
        }
        public ConvertResult<uint> From(ConvertContext context, DateTime input) => context.InvalidCastException(input, TypeFriendlyName);
        public ConvertResult<uint> From(ConvertContext context, string input)
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
        public ConvertResult<uint> From(ConvertContext context, object input) => context.InvalidCastException(input, TypeFriendlyName);
        public ConvertResult<uint> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(uint))
            {
                return context.InvalidCastException(input, TypeFriendlyName);
            }
            return BitConverter.ToUInt32(input.Slice(sizeof(uint)), 0);
        }
    }
}