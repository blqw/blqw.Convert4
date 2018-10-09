using System.Security.Cryptography;
using System;
using System.Globalization;
using static System.UInt16;
using blqw.ConvertServices;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="ushort"/> 转换器
    /// </summary>
    public class UInt16Convertor : BaseConvertor<ushort>
                                , IFromConvertible<ushort>
                                , IFrom<object, ushort>
                                , IFrom<byte[], ushort>
    {
        public ushort From(ConvertContext context, bool input) => input ? (ushort)1 : (ushort)0;
        public ushort From(ConvertContext context, char input) => (ushort)input;
        public ushort From(ConvertContext context, sbyte input) => (ushort)input;
        public ushort From(ConvertContext context, byte input) => (ushort)input;
        public ushort From(ConvertContext context, short input) => (ushort)input;
        public ushort From(ConvertContext context, ushort input) => (ushort)input;
        public ushort From(ConvertContext context, int input) => (ushort)input;
        public ushort From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                context.OverflowException($"{input} > {MaxValue}");
                return default;
            }
            return (ushort)input;
        }
        public ushort From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
                return 0;
            }
            return (ushort)input;
        }
        public ushort From(ConvertContext context, ulong input)
        {
            if (input > (int)MaxValue)
            {
                context.OverflowException($"{input} > {MaxValue}");
                return 0;
            }
            return (ushort)input;
        }
        public ushort From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
                return 0;
            }
            return (ushort)input;
        }
        public ushort From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
                return 0;
            }
            return (ushort)input;
        }
        public ushort From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
                return 0;
            }
            return decimal.ToUInt16(input);
        }
        public ushort From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public ushort From(ConvertContext context, string input)
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
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public ushort From(ConvertContext context, object input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public ushort From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(ushort))
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return BitConverter.ToUInt16(input.Slice(sizeof(ushort)), 0);
        }
    }
}