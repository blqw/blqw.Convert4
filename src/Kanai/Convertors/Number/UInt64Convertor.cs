using blqw.Kanai.Extensions;
using blqw.Kanai.Interface.From;
using System;
using System.Globalization;
using static System.UInt64;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// <seealso cref="ulong"/> 转换器
    /// </summary>
    public class UInt64Convertor : BaseConvertor<ulong>
                                , IFromConvertible<ulong>
                                , IFrom<object, ulong>
                                , IFrom<byte[], ulong>
    {
        public UInt64Convertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ConvertResult<ulong> From(ConvertContext context, bool input) => input ? (ulong)1 : (ulong)0;
        public ConvertResult<ulong> From(ConvertContext context, char input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, sbyte input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, byte input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, short input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, ushort input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, int input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, uint input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, long input)
        {
            if (input < 0)
            {
                return this.Overflow($"{input} < {MinValue}", context);
            }
            return (ulong)input;
        }
        public ConvertResult<ulong> From(ConvertContext context, ulong input) => (ulong)input;
        public ConvertResult<ulong> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (ulong)input;
        }
        public ConvertResult<ulong> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (ulong)input;
        }
        public ConvertResult<ulong> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return decimal.ToUInt64(input);
        }
        public ConvertResult<ulong> From(ConvertContext context, DateTime input) => this.Fail(context, input);
        public ConvertResult<ulong> From(ConvertContext context, string input)
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
                                return System.Convert.ToUInt64(s.Substring(2), 2);
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
        public ConvertResult<ulong> From(ConvertContext context, object input) => this.Fail(context, input);
        public ConvertResult<ulong> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(ulong))
            {
                return this.Fail(context, input);
            }
            return BitConverter.ToUInt64(input.Slice(sizeof(ulong)), 0);
        }
    }
}
