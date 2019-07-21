using System;
using System.Globalization;
using static System.Single;
using blqw.Kanai;
using blqw.Kanai.Convertors;
using blqw.Kanai.Extensions;
using blqw.Kanai.Froms;

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
                return this.Overflow($"{input} > {MaxValue}", context.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(ConvertContext context, decimal input) => decimal.ToSingle(input);
        public ConvertResult<float> From(ConvertContext context, DateTime input) => this.Fail(input, context.CultureInfo);
        public ConvertResult<float> From(ConvertContext context, string input)
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
                            if (long.TryParse(s.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out var result0))
                            {
                                return result0;
                            }
                            break;
                        case 'b':
                        case 'B':
                            try
                            {
                                return Convert.ToInt64(s.Substring(2), 2);
                            }
                            catch (Exception e)
                            {
                                return this.Error(e, context.CultureInfo);
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
            return this.Fail(input, context.CultureInfo);
        }

        public ConvertResult<float> From(ConvertContext context, object input) => this.Fail(input, context.CultureInfo);

        public ConvertResult<float> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(float))
            {
                return this.Fail(input, context.CultureInfo);
            }
            return BitConverter.ToSingle(input.Slice(sizeof(float)), 0);
        }
    }
}