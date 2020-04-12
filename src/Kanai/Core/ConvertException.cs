using blqw.Kanai.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换异常
    /// </summary>
    [Serializable]
    [DebuggerDisplay("[{ExceptionCount}]{FullMessage}")]
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
        public string FullMessage
        {
            get
            {
                var buffer = new StringBuilder();
                AppendMessage(buffer, this, 0);
                return buffer.ToString();
            }
        }

        private void AppendMessage(StringBuilder buffer, Exception ex, int tabs)
        {
            //buffer.Append(' ', tabs);
            buffer.AppendLine(ex.Message);
            if (ex is AggregateException aggregate)
            {
                var exs = aggregate.InnerExceptions;
                if (exs == null || exs.Count == 0)
                {
                    return;
                }

                for (var i = 0; i < exs.Count; i++)
                {
                    buffer.Append(' ', tabs + 2);
                    buffer.Append("Reason");
                    if (exs.Count > 1)
                    {
                        buffer.Append(i + 1);
                    }
                    buffer.Append(": ");
                    AppendMessage(buffer, exs[i], tabs + 2);
                }
                return;
            }


            var innex = ex.InnerException;
            if (innex is null)
            {
                return;
            }
            buffer.Append(' ', tabs + 2);
            buffer.Append("Reason: ");
            AppendMessage(buffer, ex.InnerException, tabs + 2);
        }


        /// <summary>
        /// 内部异常数
        /// </summary>
        public int ExceptionCount => (InnerExceptions?.Count ?? 0);

        public override string ToString() => FullMessage;
    }
}
