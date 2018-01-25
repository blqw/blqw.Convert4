using System;
using System.Collections.Generic;

namespace blqw
{
    /// <summary>
    /// 转换异常
    /// </summary>
    public sealed class ConvertError
    {
        /// <summary>
        /// 初始化异常
        /// </summary>
        public ConvertError() { }

        /// <summary>
        /// 初始化异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public ConvertError(string message) => Message = message;

        private string _message;

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <returns>返回用户设置的message值,如果没有指定,则返回第一个Exception的Message</returns>
        public string Message
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_message) && Exceptions.Count > 0)
                {
                    return Exceptions[Exceptions.Count - 1].Message;
                }
                return _message;
            }
            set => _message = value;
        }

        /// <summary>
        /// 异常集合
        /// </summary>
        /// <returns></returns>
        public List<Exception> Exceptions { get; } = new List<Exception>();

        public Exception Exception
        {
            get
            {
                if(Exceptions.Count == 0)
                {
                    if (string.IsNullOrWhiteSpace(_message))
                    {
                        return null;
                    }
                    return new InvalidCastException(_message);
                }
                if(Exceptions.Count == 1)
                {
                    return Exceptions[0];
                }
                return new AggregateException(Message, Exceptions);
            }
        }

        public void TryThrow()
        {
            var ex = Exception;
            if (ex != null)
            {
                throw ex;
            }
        }
    }
}