using blqw.ConvertServices;
using System;

namespace blqw.Convertors
{

    /// <summary>
    /// <seealso cref="Uri" /> 转换器
    /// </summary>
    class UriConvertor : BaseConvertor<Uri>, IFrom<string, Uri>
    {
        public Uri From(ConvertContext context, string input)
        {
            if (input == null)
            {
                return null;
            }
            Uri result;
            input = input.TrimStart();
            if ((input.Length > 10) && (input[6] != '/'))
            {
                if (Uri.TryCreate("http://" + input, UriKind.Absolute, out result))
                {
                    return result;
                }
            }

            if (Uri.TryCreate(input, UriKind.Absolute, out result))
            {
                return result;
            }

            context.OverflowException($"{input:!} {"不是一个有效的url"}");
            return null;
        }
    }
}
