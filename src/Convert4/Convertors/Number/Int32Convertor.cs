using blqw.ConvertServices;
using System;
using System.Globalization;
using static System.Int32;

namespace blqw.Convertors
{
    class Int32Convertor : BaseConvertor<int>, IFromConvertible<int>, IFrom<object, int>, IFrom<byte[], int>
    {
        public int From(ConvertContext context, bool input) => input ? 1 : 0;
        public int From(ConvertContext context, char input) => input;
        public int From(ConvertContext context, sbyte input) => input;
        public int From(ConvertContext context, byte input) => input;
        public int From(ConvertContext context, short input) => input;
        public int From(ConvertContext context, ushort input) => input;
        public int From(ConvertContext context, int input) => input;
        public int From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (int)input;
        }

        public int From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (int)input;
        }

        public int From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (int)input;
        }
        public int From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (int)input;
        }
        public int From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (int)input;
        }
        public int From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return decimal.ToInt32(input);
        }
        public int From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public int From(ConvertContext context, string input)
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
            return 0;
        }
        public int From(ConvertContext context, object input) => input?.GetHashCode() ?? default;
        public int From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(int))
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return BitConverter.ToInt32(input.Fill(sizeof(int)), 0);
        }
    }
}
