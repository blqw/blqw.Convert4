using blqw.Kanai.Extensions;
using blqw.Kanai.Interface.From;
using System;
using System.Globalization;
using static System.SByte;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// <seealso cref="sbyte"/> 转换器
    /// </summary>
    public class SBtyeConvertor : BaseConvertor<sbyte>
                                , IFromConvertible<sbyte>
                                , IFrom<object, sbyte>
    {
        public SBtyeConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ConvertResult<sbyte> From(ConvertContext context, bool input) => input ? (sbyte)1 : (sbyte)0;
        public ConvertResult<sbyte> From(ConvertContext context, char input) => (sbyte)input;
        public ConvertResult<sbyte> From(ConvertContext context, sbyte input) => (sbyte)input;
        public ConvertResult<sbyte> From(ConvertContext context, byte input) => (sbyte)input;
        public ConvertResult<sbyte> From(ConvertContext context, short input) => (sbyte)input;
        public ConvertResult<sbyte> From(ConvertContext context, ushort input) => (sbyte)input;
        public ConvertResult<sbyte> From(ConvertContext context, int input) => (sbyte)input;
        public ConvertResult<sbyte> From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(ConvertContext context, ulong input)
        {
            if (input > (int)MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return decimal.ToSByte(input);
        }
        public ConvertResult<sbyte> From(ConvertContext context, DateTime input) => this.Fail(context, input);

        public ConvertResult<sbyte> From(ConvertContext context, string input)
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
                                return System.Convert.ToSByte(s.Substring(2), 2);
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
        public ConvertResult<sbyte> From(ConvertContext context, object input) => this.Fail(context, input);
    }
}
