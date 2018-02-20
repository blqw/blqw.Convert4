using System;
using static System.Char;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="char" /> 转换器
    /// </summary>
    public class CharConvertor : BaseConvertor<char>, IFromConvertible<char>, IFrom<byte[],char>
    {
        public char From(ConvertContext context, bool input) => input ? 'Y' : 'N';
        public char From(ConvertContext context, char input) => input;
        public char From(ConvertContext context, sbyte input)
        {
            if (input < MinValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, byte input) => (char)input;
        public char From(ConvertContext context, short input)
        {
            if (input < MinValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, ushort input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, int input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, uint input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, long input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, ulong input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, float input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, double input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, decimal input)
        {
            if (input < MinValue || input > MaxValue)
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return (char)input;
        }
        public char From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }
        public char From(ConvertContext context, string input)
        {
            if (input?.Length == 1)
            {
                return input[0];
            }
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }

        public char From(ConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(char))
            {
                context.InvalidCastException(input, TypeFriendlyName);
                return default;
            }
            return BitConverter.ToChar(input.Fill(sizeof(char)), 0);
        }
    }
}