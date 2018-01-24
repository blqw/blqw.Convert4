using blqw.Services;
using System;
using System.Globalization;

namespace blqw.Convertors
{
    class Int32Convertor : BaseConvertor<int>, IFromConvertible<int>, IFrom<int, object>
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
            if (input > 2147483647)
            {
                context.Error("值超过限制");
                return 0;
            }
            return (int)input;
        }

        public int From(ConvertContext context, long input)
        {
            if ((input < -2147483648) || (input > 2147483647))
            {
                context.Error("值超过限制");
                return 0;
            }
            return (int)input;
        }

        public int From(ConvertContext context, ulong input)
        {
            if (input > 2147483647)
            {
                context.Error("值超过限制");
                return 0;
            }
            return (int)input;
        }
        public int From(ConvertContext context, float input)
        {
            if ((input < -2147483648) || (input > 2147483647))
            {
                context.Error("值超过限制");
                return 0;
            }
            return (int)input;
        }
        public int From(ConvertContext context, double input)
        {
            if ((input < -2147483648) || (input > 2147483647))
            {
                context.Error("值超过限制");
                return 0;
            }
            return (int)input;
        }
        public int From(ConvertContext context, decimal input)
        {
            if ((input < -2147483648) || (input > 2147483647))
            {
                context.Error("值超过限制");
                return 0;
            }
            return (int)input;
        }
        public int From(ConvertContext context, DateTime input)
        {
            context.Error(input, TypeFriendlyName);
            return 0;
        }
        public int From(ConvertContext context, string input)
        {
            if (int.TryParse(input, out var result))
            {
                return result;
            }
            var hex = input.ToHex();
            if (hex != null && int.TryParse(hex, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out result))
            {
                return result;
            }
            context.Error(input, TypeFriendlyName);
            return 0;
        }
        public int From(ConvertContext context, object input) => input?.GetHashCode() ?? int.MinValue;
    }
}
