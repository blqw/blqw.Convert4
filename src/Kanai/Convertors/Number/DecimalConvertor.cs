﻿using blqw.Kanai.Extensions;
using blqw.Kanai.Interface.From;
using System;
using System.Globalization;
using static System.Decimal;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// <seealso cref="decimal" /> 转换器
    /// </summary>
    public class DecimalConvertor : BaseConvertor<decimal>, IFromConvertible<decimal>, IFrom<byte[], decimal>, IFrom<Guid, decimal>
    {
        public DecimalConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

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
        public ConvertResult<decimal> From(ConvertContext context, DateTime input) => this.Fail(context, input);
        public ConvertResult<decimal> From(ConvertContext context, string input)
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
                                return System.Convert.ToInt64(s.Substring(2), 2);
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

        public ConvertResult<decimal> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(decimal))
            {
                return this.Fail(context, input);
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
