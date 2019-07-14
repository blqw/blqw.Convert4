using blqw.Kanai.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换异常
    /// </summary>
    [Serializable]
    public class ConvertException : AggregateException
    {
        public ConvertException(Type outputType, object origin, IEnumerable<Exception> exceptions) :
            base(GetMessage(outputType, origin, null), exceptions)
        {
        }

        private static string GetMessage(Type outputType, object origin, CultureInfo cultureInfo)
        {
            if (outputType == null)
            {
                throw new ArgumentNullException(nameof(outputType));
            }
            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            var text = (origin as IConvertible)?.ToString(null)
                       ?? (origin as IFormattable)?.ToString(null, null);
            var outputTypeName = outputType.GetFriendlyName();
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
