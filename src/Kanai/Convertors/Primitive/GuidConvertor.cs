using blqw.Kanai;
using blqw.Kanai.Convertors;
using blqw.Kanai.Extensions;
using blqw.Kanai.Froms;
using System;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="Guid" /> 转换器
    /// </summary>
    public class GuidConvertor : BaseConvertor<Guid>, IFrom<string, Guid>, IFrom<byte[], Guid>, IFrom<decimal, Guid>
    {
        public ConvertResult<Guid> From(ConvertContext context, string input)
        {
            if (input.Length == 0)
            {
                return this.Fail(input, context);
            }
            if (Guid.TryParse(input, out var result))
            {
                return result;
            }
            return this.Fail(input, context);
        }

        public ConvertResult<Guid> From(ConvertContext context, decimal input)
        {
            var arr = decimal.GetBits(input);
            var bytes = new byte[16];
            Buffer.BlockCopy(arr, 0, bytes, 0, 16);
            return new Guid(bytes);
        }

        public ConvertResult<Guid> From(ConvertContext context, byte[] input)
        {
            if (input?.Length == 16)
            {
                return new Guid(input);
            }
            return this.Fail(input, context);
        }
    }
}
