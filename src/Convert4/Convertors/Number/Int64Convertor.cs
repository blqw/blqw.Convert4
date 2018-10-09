﻿using blqw.ConvertServices;
using System;
using System.Globalization;
using static System.Int64;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="long"/> 转换器
    /// </summary>
    public class Int64Convertor : BaseConvertor<long>
                                , IFromConvertible<long>
                                , IFrom<object, long>
                                , IFrom<byte[], long>
    {
        public long From(ConvertContext context, bool input) => input ? (long)1 : (long)0;
        public long From(ConvertContext context, char input) => (long)input;
        public long From(ConvertContext context, sbyte input) => (long)input;
        public long From(ConvertContext context, byte input) => (long)input;
        public long From(ConvertContext context, short input) => (long)input;
        public long From(ConvertContext context, ushort input) => (long)input;
        public long From(ConvertContext context, int input) => (long)input;
        public long From(ConvertContext context, uint input) => (long)input;
        public long From(ConvertContext context, long input) => (long)input;
        public long From(ConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                context.OverflowException($"{input} > {MaxValue}");
                return 0;
            }
            return (long)input;
        }
        public long From(ConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
                return 0;
            }
            return (long)input;
        }
        public long From(ConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
                return 0;
            }
            return (long)input;
        }
        public long From(ConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                context.OverflowException(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}");
                return 0;
            }
            return decimal.ToInt64(input);
        }
        public long From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public long From(ConvertContext context, string input)
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
        public long From(ConvertContext context, object input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public long From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(long))
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return BitConverter.ToInt64(input.Slice(sizeof(long)), 0);
        }
    }
}