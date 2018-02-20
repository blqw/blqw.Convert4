using System;
using System.Globalization;
using static System.Single;

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
        public float From(ConvertContext context, bool input) => input ? (float)1 : (float)0;
        public float From(ConvertContext context, char input) => (float)input;
        public float From(ConvertContext context, sbyte input) => (float)input;
        public float From(ConvertContext context, byte input) => (float)input;
        public float From(ConvertContext context, short input) => (float)input;
        public float From(ConvertContext context, ushort input) => (float)input;
        public float From(ConvertContext context, int input) => (float)input;
        public float From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                context.InvalidCastException($"值超过限制");
                return default;
            }
            return (float)input;
        }
        public float From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (float)input;
        }
        public float From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (float)input;
        }
        public float From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (float)input;
        }
        public float From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.InvalidCastException($"值超过限制");
                return 0;
            }
            return (float)input;
        }
        public float From(ConvertContext context, decimal input) => decimal.ToSingle(input);
        public float From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public float From(ConvertContext context, string input)
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
        public float From(ConvertContext context, object input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public float From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(float))
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return BitConverter.ToSingle(input.Fill(sizeof(float)), 0);
        }
    }
}