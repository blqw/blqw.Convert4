using blqw.Kanai.Interface;
using System;
using System.Net;
using System.Text;

namespace blqw.Kanai
{
    class StringTranslator : ITranslator
    {
        public bool CanTranslate(Type type) =>
            typeof(IFormattable).IsAssignableFrom(type)
            || type == typeof(StringBuilder)
            || type == typeof(Uri)
            || type == typeof(IPAddress);

        public object Translate(ConvertContext context, object input)
        {
            if (input is StringBuilder
                || input is Uri
                || input is IPAddress
                || input is IFormattable)
            {
                var result = context.Convert<string>(input);
                if (result.Success)
                {
                    return result.OutputValue;
                }
            }
            return null;
        }

    }
}
