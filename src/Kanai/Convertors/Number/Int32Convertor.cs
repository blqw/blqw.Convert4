using blqw.Kanai.Extensions;
using blqw.Kanai.Interface.From;
using System;
using System.Globalization;
using static System.Int32;

namespace blqw.Kanai.Convertors
{
    class Int32Convertor : BaseConvertor<int>, IFromConvertible<int>, IFrom<object, int>, IFrom<byte[], int>
    {
        public Int32Convertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

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
                return this.Overflow($"{input} > {MaxValue}", context);
            }
            return (int)input;
        }

        public ConvertResult<int> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (int)input;
        }

        public ConvertResult<int> From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context);
            }
            return (int)input;
        }
        public ConvertResult<int> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (int)input;
        }
        public ConvertResult<int> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (int)input;
        }
        public ConvertResult<int> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return decimal.ToInt32(input);
        }
        public ConvertResult<int> From(ConvertContext context, DateTime input) => this.Fail(context, input);
        public ConvertResult<int> From(ConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
            if (TryParse(s, NumberStyles.Any, context.NumberFormatInfo ?? NumberFormatInfo.CurrentInfo, out var result))
            {
                return result;
            }
            if (s.Length > 2)
            {
                if (s[0] == '0')
                {
                    switch (s[1])
                    {
                        case 'x':
                        case 'X':
                            if (TryParse(s.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out result))
                            {
                                return result;
                            }
                            break;
                        case 'b':
                        case 'B':
                            try
                            {
                                return Convert.ToInt32(s.Substring(2), 2);
                            }
                            catch (Exception e)
                            {
                                return this.Error(e, context);
                            }
                        default:
                            break;
                    }
                }
                else if (s[0] == '&' && (s[1] == 'H' || s[1] == 'h'))
                {
                    if (TryParse(s.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out result))
                    {
                        return result;
                    }
                }
            }
            return this.Fail(context, input);
        }
        public ConvertResult<int> From(ConvertContext context, object input) => input?.GetHashCode() ?? default;
        public ConvertResult<int> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(int))
            {
                return this.Fail(context, input);
            }
            return BitConverter.ToInt32(input.Slice(sizeof(int)), 0);
        }
    }
}
