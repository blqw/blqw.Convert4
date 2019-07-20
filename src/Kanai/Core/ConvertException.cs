using blqw.Kanai.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换异常
    /// </summary>
    [Serializable]
    public class ConvertException : AggregateException
    {
        public ConvertException(Type outputType, object origin, IEnumerable<Exception> exceptions, CultureInfo cultureInfo)
            : base(GetMessage(outputType.GetFriendlyName(), origin, cultureInfo), exceptions)
        {
        }

        public ConvertException(string outputTypeName, object origin, IEnumerable<Exception> exceptions, CultureInfo cultureInfo)
            : base(GetMessage(outputTypeName, origin, cultureInfo), exceptions)
        {
        }

        public static InvalidCastException InvalidCast(Type outputType, object origin, CultureInfo cultureInfo)
            => new InvalidCastException(GetMessage(outputType.GetFriendlyName(), origin, cultureInfo));

        public static InvalidCastException InvalidCast(string outputTypeName, object origin, CultureInfo cultureInfo)
            => new InvalidCastException(GetMessage(outputTypeName, origin, cultureInfo));


        private static string GetMessage(string outputTypeName, object origin, CultureInfo cultureInfo)
        {
            if (outputTypeName == null)
            {
                outputTypeName = "`null`";
            }

            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            var text = (origin as IConvertible)?.ToString(null)
                       ?? (origin as IFormattable)?.ToString(null, null);
            if (origin == null)
            {
                text = SR.CANT_CONVERT.Localize(cultureInfo, "null", outputTypeName);
            }
            else if (text == null)
            {
                text = SR.CANT_CONVERT.Localize(cultureInfo, origin.GetType().GetFriendlyName(), outputTypeName);
            }
            else
            {
                text = SR.VALUE_CANT_CONVERT.Localize(cultureInfo, text, origin.GetType().GetFriendlyName(), outputTypeName);
            }
            return text;
        }


        protected ConvertException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        /// <summary>
        /// 完整的异常文本
        /// </summary>
        public string FullMessage => string.Join(Environment.NewLine, InnerExceptions.Select(x => x.Message));
        /// <summary>
        /// 内部异常数
        /// </summary>
        public int ExceptionCount => (InnerExceptions?.Count ?? 0);

    }
}
