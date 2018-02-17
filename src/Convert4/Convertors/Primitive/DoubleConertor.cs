using System;
using System.Globalization;
using static System.Double;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="double" /> 转换器
    /// </summary>
    public class DoubleConertor : BaseConvertor<double>, IFromConvertible<double>, IFrom<double, byte[]>
    {
        public double From(ConvertContext context, bool input) => input ? 1 : 0;
        public double From(ConvertContext context, char input) => input;
        public double From(ConvertContext context, sbyte input) => input;
        public double From(ConvertContext context, byte input) => input;
        public double From(ConvertContext context, short input) => input;
        public double From(ConvertContext context, ushort input) => input;
        public double From(ConvertContext context, int input) => input;
        public double From(ConvertContext context, uint input) => input;
        public double From(ConvertContext context, long input) => input;
        public double From(ConvertContext context, ulong input) => input;
        public double From(ConvertContext context, float input) => input;
        public double From(ConvertContext context, double input) => input;
        public double From(ConvertContext context, decimal input) => decimal.ToDouble(input);
        public double From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public double From(ConvertContext context, string input)
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
        public double From(ConvertContext context, byte[] input)
        {
            if (input?.Length != 8)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return BitConverter.ToDouble(input, 0);
        }
    }
}