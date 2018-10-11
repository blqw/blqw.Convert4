using blqw.ConvertServices;
using System;
using System.Globalization;
using static System.Int32;

namespace blqw.Convertors
{
    class Int32Convertor : BaseConvertor<int>, IFromConvertible<int>, IFrom<object, int>, IFrom<byte[], int>
    {
        public ConvertResult<int> From(ConvertContext context, bool input) => input ? 1 : 0;
        public ConvertResult<int> From(ConvertContext context, char input) => input;
        public ConvertResult<int> From(ConvertContext context, sbyte input) => input;
        public ConvertResult<int> From(ConvertContext context, byte input) => input;
        public ConvertResult<int> From(ConvertContext context, short input) => input;
        public ConvertResult<int> From(ConvertContext context, ushort input) => input;
        public ConvertResult<int> From(ConvertContext context, int input) => input;
        public ConvertResult<int> From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return context.OverflowException($"{input} > {MaxValue}");
            }
            return (int)input;
        }

        public ConvertResult<int> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (int)input;
        }

        public ConvertResult<int> From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return context.OverflowException($"{input} > {MaxValue}");
            }
            return (int)input;
        }
        public ConvertResult<int> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (int)input;
        }
        public ConvertResult<int> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (int)input;
        }
        public ConvertResult<int> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return decimal.ToInt32(input);
        }
        public ConvertResult<int> From(ConvertContext context, DateTime input) => context.InvalidCastException(input, TypeFriendlyName);
        public ConvertResult<int> From(ConvertContext context, string input)
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
        public ConvertResult<int> From(ConvertContext context, object input) => input?.GetHashCode() ?? default;
        public ConvertResult<int> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(int))
            {
                return context.InvalidCastException(input, TypeFriendlyName);
            }
            return BitConverter.ToInt32(input.Slice(sizeof(int)), 0);
        }
    }
}
