using blqw.Kanai.Extensions;
using blqw.Kanai.Interface.From;
using System;
using System.Globalization;
using static System.UInt16;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// <seealso cref="ushort"/> 转换器
    /// </summary>
    public class UInt16Convertor : BaseConvertor<ushort>
                                , IFromConvertible<ushort>
                                , IFrom<object, ushort>
                                , IFrom<byte[], ushort>
    {
        public UInt16Convertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ConvertResult<ushort> From(ConvertContext context, bool input) => input ? (ushort)1 : (ushort)0;
        public ConvertResult<ushort> From(ConvertContext context, char input) => (ushort)input;
        public ConvertResult<ushort> From(ConvertContext context, sbyte input) => (ushort)input;
        public ConvertResult<ushort> From(ConvertContext context, byte input) => (ushort)input;
        public ConvertResult<ushort> From(ConvertContext context, short input) => (ushort)input;
        public ConvertResult<ushort> From(ConvertContext context, ushort input) => (ushort)input;
        public ConvertResult<ushort> From(ConvertContext context, int input) => (ushort)input;
        public ConvertResult<ushort> From(ConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(ConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(ConvertContext context, ulong input)
        {
            if (input > (int)MaxValue)
            {
                return this.Overflow($"{input} > {MaxValue}", context);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return this.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context);
            }
            return decimal.ToUInt16(input);
        }
        public ConvertResult<ushort> From(ConvertContext context, DateTime input) => this.Fail(context, input);
        public ConvertResult<ushort> From(ConvertContext context, string input)
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
                                return System.Convert.ToUInt16(s.Substring(2), 2);
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
        public ConvertResult<ushort> From(ConvertContext context, object input) => this.Fail(context, input);
        public ConvertResult<ushort> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(ushort))
            {
                return this.Fail(context, input);
            }
            return BitConverter.ToUInt16(input.Slice(sizeof(ushort)), 0);
        }
    }
}
