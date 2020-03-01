using blqw.Kanai;
using blqw.Kanai.Convertors;
using blqw.Kanai.Extensions;
using blqw.Kanai.Froms;
using System;
using System.Globalization;
using static System.Double;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="double" /> 转换器
    /// </summary>
    public class DoubleConertor : BaseConvertor<double>, IFromConvertible<double>, IFrom<byte[], double>
    {
        public ConvertResult<double> From(ConvertContext context, bool input) => input ? 1 : 0;
        public ConvertResult<double> From(ConvertContext context, char input) => input;
        public ConvertResult<double> From(ConvertContext context, sbyte input) => input;
        public ConvertResult<double> From(ConvertContext context, byte input) => input;
        public ConvertResult<double> From(ConvertContext context, short input) => input;
        public ConvertResult<double> From(ConvertContext context, ushort input) => input;
        public ConvertResult<double> From(ConvertContext context, int input) => input;
        public ConvertResult<double> From(ConvertContext context, uint input) => input;
        public ConvertResult<double> From(ConvertContext context, long input) => input;
        public ConvertResult<double> From(ConvertContext context, ulong input) => input;
        public ConvertResult<double> From(ConvertContext context, float input) => input;
        public ConvertResult<double> From(ConvertContext context, double input) => input;
        public ConvertResult<double> From(ConvertContext context, decimal input) => decimal.ToDouble(input);
        public ConvertResult<double> From(ConvertContext context, DateTime input) => this.Fail(input, context);
        public ConvertResult<double> From(ConvertContext context, string input)
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
            return this.Fail(input, context);
        }
        public ConvertResult<double> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(double))
            {
                return this.Fail(input, context);
            }
            return BitConverter.ToDouble(input.Slice(sizeof(double)), 0);
        }
    }
}
