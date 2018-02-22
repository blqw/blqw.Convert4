using System.Security.Cryptography;
using System;
using System.Globalization;
using static System.SByte;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="sbyte"/> 转换器
    /// </summary>
    public class SBtyeConvertor : BaseConvertor<sbyte>
                                , IFromConvertible<sbyte>
                                , IFrom<object, sbyte>
    {
        public sbyte From(ConvertContext context, bool input) => input ? (sbyte)1 : (sbyte)0;
        public sbyte From(ConvertContext context, char input) => (sbyte)input;
        public sbyte From(ConvertContext context, sbyte input) => (sbyte)input;
        public sbyte From(ConvertContext context, byte input) => (sbyte)input;
        public sbyte From(ConvertContext context, short input) => (sbyte)input;
        public sbyte From(ConvertContext context, ushort input) => (sbyte)input;
        public sbyte From(ConvertContext context, int input) => (sbyte)input;
        public sbyte From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                context.InvalidCastException($"值超过限制");
                return default;
            }
            return (sbyte)input;
        }
        public sbyte From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (sbyte)input;
        }
        public sbyte From(ConvertContext context, ulong input)
        {
            if (input > (int)MaxValue)
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (sbyte)input;
        }
        public sbyte From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (sbyte)input;
        }
        public sbyte From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (sbyte)input;
        }
        public sbyte From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return decimal.ToSByte(input);
        }
        public sbyte From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public sbyte From(ConvertContext context, string input)
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
        public sbyte From(ConvertContext context, object input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
    }
}