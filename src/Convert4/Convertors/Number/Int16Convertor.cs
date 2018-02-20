using System.Security.Cryptography;
using System;
using System.Globalization;
using static System.Int16;

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
        public short From(ConvertContext context, bool input) => input ? (short)1 : (short)0;
        public short From(ConvertContext context, char input) => (short)input;
        public short From(ConvertContext context, sbyte input) => (short)input;
        public short From(ConvertContext context, byte input) => (short)input;
        public short From(ConvertContext context, short input) => (short)input;
        public short From(ConvertContext context, ushort input) => (short)input;
        public short From(ConvertContext context, int input) => (short)input;
        public short From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                context.InvalidCastException($"值超过限制");
                return default;
            }
            return (short)input;
        }
        public short From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (short)input;
        }
        public short From(ConvertContext context, ulong input)
        {
            if (input > (int)MaxValue)
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (short)input;
        }
        public short From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (short)input;
        }
        public short From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (short)input;
        }
        public short From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return decimal.ToInt16(input);
        }
        public short From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public short From(ConvertContext context, string input)
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
        public short From(ConvertContext context, object input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public short From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(short))
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return BitConverter.ToInt16(input.Fill(sizeof(short)), 0);
        }
    }
}