using System;
using System.Globalization;
using static System.Decimal;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="decimal" /> 转换器
    /// </summary>
    public class DecimalConvertor : BaseConvertor<decimal>, IFromConvertible<decimal>, IFrom<decimal, byte[]>, IFrom<decimal, Guid>
    {
        public decimal From(ConvertContext context, bool input) => input ? One : Zero;
        public decimal From(ConvertContext context, char input) => input;
        public decimal From(ConvertContext context, sbyte input) => input;
        public decimal From(ConvertContext context, byte input) => input;
        public decimal From(ConvertContext context, short input) => input;
        public decimal From(ConvertContext context, ushort input) => input;
        public decimal From(ConvertContext context, int input) => input;
        public decimal From(ConvertContext context, uint input) => input;
        public decimal From(ConvertContext context, long input) => input;
        public decimal From(ConvertContext context, ulong input) => input;
        public decimal From(ConvertContext context, float input) => (decimal)input;
        public decimal From(ConvertContext context, double input) => (decimal)input;
        public decimal From(ConvertContext context, decimal input) => input;
        public decimal From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public decimal From(ConvertContext context, string input)
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

        public decimal From(ConvertContext context, byte[] input)
        {
            if (input?.Length == 16)
            {
                var arr2 = new int[4];
                Buffer.BlockCopy(input, 0, arr2, 0, 16);
                return new decimal(arr2);
            }
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public decimal From(ConvertContext context, Guid input)
        {
            var bytes = input.ToByteArray();
            var arr2 = new int[4];
            Buffer.BlockCopy(bytes, 0, arr2, 0, 16);
            return new decimal(arr2);
        }
    }
}