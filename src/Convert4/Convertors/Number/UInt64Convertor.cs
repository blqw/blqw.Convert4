using System.Security.Cryptography;
using System;
using System.Globalization;
using static System.UInt64;

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
        public ulong From(ConvertContext context, bool input) => input ? (ulong)1 : (ulong)0;
        public ulong From(ConvertContext context, char input) => (ulong)input;
        public ulong From(ConvertContext context, sbyte input) => (ulong)input;
        public ulong From(ConvertContext context, byte input) => (ulong)input;
        public ulong From(ConvertContext context, short input) => (ulong)input;
        public ulong From(ConvertContext context, ushort input) => (ulong)input;
        public ulong From(ConvertContext context, int input) => (ulong)input;
        public ulong From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                context.InvalidCastException($"值超过限制");
                return default;
            }
            return (ulong)input;
        }
        public ulong From(ConvertContext context, long input)
        {
            if (input < 0)
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (ulong)input;
        }
        public ulong From(ConvertContext context, ulong input) => (ulong)input;
        public ulong From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (ulong)input;
        }
        public ulong From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (ulong)input;
        }
        public ulong From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return decimal.ToUInt64(input);
        }
        public ulong From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public ulong From(ConvertContext context, string input)
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
        public ulong From(ConvertContext context, object input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public ulong From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(ulong))
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return BitConverter.ToUInt64(input.Fill(sizeof(ulong)), 0);
        }
    }
}