using blqw.ConvertServices;
using System;
using System.Globalization;
using static System.Decimal;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="decimal" /> 转换器
    /// </summary>
    public class DecimalConvertor : BaseConvertor<decimal>, IFromConvertible<decimal>, IFrom<byte[], decimal>, IFrom<Guid, decimal>
    {
        public ConvertResult<decimal> From(ConvertContext context, bool input) => input ? One : Zero;
        public ConvertResult<decimal> From(ConvertContext context, char input) => input;
        public ConvertResult<decimal> From(ConvertContext context, sbyte input) => input;
        public ConvertResult<decimal> From(ConvertContext context, byte input) => input;
        public ConvertResult<decimal> From(ConvertContext context, short input) => input;
        public ConvertResult<decimal> From(ConvertContext context, ushort input) => input;
        public ConvertResult<decimal> From(ConvertContext context, int input) => input;
        public ConvertResult<decimal> From(ConvertContext context, uint input) => input;
        public ConvertResult<decimal> From(ConvertContext context, long input) => input;
        public ConvertResult<decimal> From(ConvertContext context, ulong input) => input;
        public ConvertResult<decimal> From(ConvertContext context, float input) => (decimal)input;
        public ConvertResult<decimal> From(ConvertContext context, double input) => (decimal)input;
        public ConvertResult<decimal> From(ConvertContext context, decimal input) => input;
        public ConvertResult<decimal> From(ConvertContext context, DateTime input) => context.InvalidCastException(input, TypeFriendlyName);
        public ConvertResult<decimal> From(ConvertContext context, string input)
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

        public ConvertResult<decimal> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(decimal))
            {
                return context.InvalidCastException(input, TypeFriendlyName);
            }
            var bytes = input.Slice(sizeof(decimal));
            var arr2 = new int[4];
            Buffer.BlockCopy(bytes, 0, arr2, 0, sizeof(decimal));
            return new decimal(arr2);
        }
        public ConvertResult<decimal> From(ConvertContext context, Guid input)
        {
            var bytes = input.ToByteArray();
            var arr2 = new int[4];
            Buffer.BlockCopy(bytes, 0, arr2, 0, sizeof(decimal));
            return new decimal(arr2);
        }
    }
}