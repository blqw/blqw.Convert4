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
        public byte From(ConvertContext context, bool input) => input ? (byte)1 : (byte)0;
        public byte From(ConvertContext context, char input) => (byte)input;
        public byte From(ConvertContext context, sbyte input)
        {
            if (input < MinValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, byte input) => input;
        public byte From(ConvertContext context, short input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, ushort input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, int input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, uint input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, long input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, ulong input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, float input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, double input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, decimal input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (byte)input;
        }
        public byte From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public byte From(ConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
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

    }
}