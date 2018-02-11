using System;
using System.Collections.Generic;
using System.Linq;

namespace blqw
{
    /// <summary>
    /// 转换错误实体
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
        public ConvertError(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Exceptions.Add(new InvalidCastException(message));
            }
        }
        /// <summary>
        /// 异常消息
        /// </summary>
        /// <returns>返回用户设置的message值,如果没有指定,则返回第一个Exception的Message</returns>
        public string Message =>
            Exceptions.LastOrDefault()?.Message;
        /// <summary>
        /// 异常集合
        /// </summary>
        /// <returns></returns>
        public List<Exception> Exceptions { get; } = new List<Exception>();
        /// <summary>
        /// 如果没有异常返回null <para />
        /// 如果只有一个异常返回具体异常  <para />
        /// 如果多余一个异常返回的集合 <seealso cref="AggregateException"/>
        /// </summary>
        public Exception Exception
        {
            get
            {
                if (Exceptions.Count == 0)
                {
                    return null;
                }
                if (Exceptions.Count == 1)
                {
                    return Exceptions[0];
                }
                return new AggregateException(Message, Exceptions);
            }
        }
        /// <summary>
        /// 如果有异常则抛出异常, 否则不执行任何操作
        /// </summary>
        public void TryThrow()
        {
            var ex = Exception;
            if (ex != null)
            {
                throw ex;
            }
        }
        #region 运算符重载
        public static Exception operator +(Exception ex, ConvertError error)
        {
            if (ex == null || error?.Exceptions == null)
            {
                return ex ?? error?.Exception;
            }

            if (error.Exceptions.Count == 0)
            {
                return ex;
            }
            if (error.Exceptions.Count == 1)
            {
                return new AggregateException(ex.Message ?? error.Message, new Exception[] { ex, error.Exceptions[0] });
            }
            var errors = new Exception[error.Exceptions.Count + 1];
            error.Exceptions.CopyTo(errors, 0);
            errors[error.Exceptions.Count] = ex;
            return new AggregateException(ex.Message, errors.Reverse());
        }
        #endregion
    }
}