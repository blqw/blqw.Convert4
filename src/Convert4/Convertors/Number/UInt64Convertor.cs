using System.Security.Cryptography;
using System;
using System.Globalization;
using static System.UInt64;
using blqw.ConvertServices;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="ulong"/> 转换器
    /// </summary>
    public class UInt64Convertor : BaseConvertor<ulong>
                                , IFromConvertible<ulong>
                                , IFrom<object, ulong>
                                , IFrom<byte[], ulong>
    {
        public ConvertResult<ulong> From(ConvertContext context, bool input) => input ? (ulong)1 : (ulong)0;
        public ConvertResult<ulong> From(ConvertContext context, char input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, sbyte input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, byte input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, short input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, ushort input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, int input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, uint input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, long input)
        {
            if (input < 0)
            {
                return context.OverflowException($"{input} < 0");
            }
            return (ulong)input;
        }
        public ConvertResult<ulong> From(ConvertContext context, ulong input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (ulong)input;
        }
        public ConvertResult<ulong> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (ulong)input;
        }
        public ConvertResult<ulong> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return decimal.ToUInt64(input);
        }
        public ConvertResult<ulong> From(ConvertContext context, DateTime input) => context.InvalidCastException(input, TypeFriendlyName);
        public ConvertResult<ulong> From(ConvertContext context, string input)
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
        public ConvertResult<ulong> From(ConvertContext context, object input) => context.InvalidCastException(input, TypeFriendlyName);
        public ConvertResult<ulong> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(ulong))
            {
                return context.InvalidCastException(input, TypeFriendlyName);
            }
            return BitConverter.ToUInt64(input.Slice(sizeof(ulong)), 0);
        }
    }
}