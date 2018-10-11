using System;
using System.Globalization;
using static System.Single;
using blqw.ConvertServices;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="float"/> 转换器
    /// </summary>
    public class SingleConvertor : BaseConvertor<float>
                                , IFromConvertible<float>
                                , IFrom<object, float>
                                , IFrom<byte[], float>
    {
        public ConvertResult<float> From(ConvertContext context, bool input) => input ? (float)1 : (float)0;
        public ConvertResult<float> From(ConvertContext context, char input) => (float)input;
        public ConvertResult<float> From(ConvertContext context, sbyte input) => (float)input;
        public ConvertResult<float> From(ConvertContext context, byte input) => (float)input;
        public ConvertResult<float> From(ConvertContext context, short input) => (float)input;
        public ConvertResult<float> From(ConvertContext context, ushort input) => (float)input;
        public ConvertResult<float> From(ConvertContext context, int input) => (float)input;

        public ConvertResult<float> From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return context.OverflowException($"{input} > {MaxValue}");
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return context.OverflowException($"{input} > {MaxValue}");
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, decimal input) => decimal.ToSingle(input);
        public ConvertResult<float> From(ConvertContext context, DateTime input) => context.InvalidCastException(input, TypeFriendlyName);
        public ConvertResult<float> From(ConvertContext context, string input)
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

        public ConvertResult<float> From(ConvertContext context, object input) => context.InvalidCastException(input, TypeFriendlyName);

        public ConvertResult<float> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(float))
            {
                return context.InvalidCastException(input, TypeFriendlyName);
            }
            return BitConverter.ToSingle(input.Slice(sizeof(float)), 0);
        }
    }
}