using blqw.Kanai.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换异常
    /// </summary>
    [Serializable]
    public class ConvertException : AggregateException
    {
        private readonly string _message;

        protected ConvertException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public ConvertException(string message, IEnumerable<Exception> innerExceptions) : base(message, innerExceptions.Select(x => x.GetRealException()))
        {
            _message = message;
        }

        public override string Message => _message;

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
