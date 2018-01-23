using System.Linq;
using System;

namespace blqw
{
    /// <summary>
    /// 转换结果
    /// </summary>
    public struct ConvertResult<T>
    {
        /// <summary>
        /// 转换成功
        /// </summary>
        /// <param name="value">返回值</param>
        public ConvertResult(T value)
        {
            Success = true;
            OutputValue = value;
            Exception = null;
        }

        private ConvertResult(bool success, T value, ConvertException ex)
        {
            Success = success;
            OutputValue = value;
            Exception = ex;
        }

        /// <summary>
        /// 转换中出现异常
        /// </summary>
        /// <param name="ex"></param>
        public static ConvertResult<T> Error(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            var e = new ConvertException();
            e.Exceptions.Add(ex);
            return new ConvertResult<T>(false, default(T), e);
        }

        /// <summary>
        /// 转换中出现异常
        /// </summary>
        /// <param name="message"></param>
        public static ConvertResult<T> Error(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            message = SR.GetString(message);
            var e = new ConvertException();
            e.Exceptions.Add(new InvalidCastException(message));
            return new ConvertResult<T>(false, default(T), e);
        }

        /// <summary>
        /// 转换中出现异常
        /// </summary>
        /// <param name="message"></param>
        public static ConvertResult<T> Error(params string[] messages)
        {
            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages));
            }
            var message = string.Concat(messages.Select(SR.GetString));
            var e = new ConvertException();
            e.Exceptions.Add(new InvalidCastException(message));
            return new ConvertResult<T>(false, default(T), e);
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool Success { get; }
        /// <summary>
        /// 转换后的输出结果
        /// </summary>
        /// <returns></returns>
        public T OutputValue { get; }
        /// <summary>
        /// 如果失败,则返回异常
        /// </summary>
        /// <returns></returns>
        public ConvertException Exception { get; }

        public static implicit operator ConvertResult(ConvertResult<T> value) => value.Success ? new ConvertResult(value.OutputValue) :  ConvertResult.Error(value.Exception);
        public static implicit operator ConvertResult<T>(ConvertResult value) => value.Success ? new ConvertResult<T>((T)value.OutputValue) : Error(value.Exception);
    }
}