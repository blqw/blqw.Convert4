using blqw.ConvertServices;
using System;
using System.Text;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="byte" /> 数组转换器
    /// </summary>
    public class BytesConvertor : BaseConvertor<byte[]>, IFromConvertible<byte[]>, IFrom<Guid, byte[]>
    {
        public byte[] From(ConvertContext context, string input) => input == null ? null : input.Length == 0 ? Array.Empty<byte>() : context.GetEncoding().GetBytes(input);
        public byte[] From(ConvertContext context, bool input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, char input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, sbyte input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, byte input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, short input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, ushort input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, int input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, uint input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, long input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, ulong input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, float input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, double input) => BitConverter.GetBytes(input);
        public byte[] From(ConvertContext context, decimal input)
        {
            var arr = decimal.GetBits(input);
            var bytes = new byte[arr.Length << 2];
            Buffer.BlockCopy(arr, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public byte[] From(ConvertContext context, DateTime input)
        {
            context.InvalidCastException(input, TypeFriendlyName);
            return default;
        }

        public byte[] From(ConvertContext context, Guid input) => input.ToByteArray();


    }
}