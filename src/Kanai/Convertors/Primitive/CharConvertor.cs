﻿using blqw.Kanai.Extensions;
using blqw.Kanai.Interface.From;
using System;
using static System.Char;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// <seealso cref="char" /> 转换器
    /// </summary>
    public class CharConvertor : BaseConvertor<char>, IFromConvertible<char>, IFrom<byte[], char>
    {
        public CharConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ConvertResult<char> From(ConvertContext context, bool input) => input ? 'Y' : 'N';
        public ConvertResult<char> From(ConvertContext context, char input) => input;
        public ConvertResult<char> From(ConvertContext context, sbyte input)
        {
            if (input < MinValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, byte input) => (char)input;
        public ConvertResult<char> From(ConvertContext context, short input)
        {
            if (input < MinValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, ushort input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, int input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, uint input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, long input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, ulong input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, float input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, double input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, decimal input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return this.Fail(context, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(ConvertContext context, DateTime input) => this.Fail(context, input);
        public ConvertResult<char> From(ConvertContext context, string input)
        {
            if (input?.Length == 1)
            {
                return input[0];
            }
            return this.Fail(context, input);
        }

        public ConvertResult<char> From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(char))
            {
                return this.Fail(context, input);
            }
            return BitConverter.ToChar(input.Slice(sizeof(char)), 0);
        }
    }
}
