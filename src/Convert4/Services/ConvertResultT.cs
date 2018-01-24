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

        public static implicit operator ConvertResult(ConvertResult<T> value) => new ConvertResult(value.Success, value.OutputValue, value.Exception);
        public static implicit operator ConvertResult<T>(ConvertResult value) => new ConvertResult<T>(value.Success, (T)value.OutputValue, value.Exception);

        public static implicit operator ConvertResult<T>(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            var e = new ConvertException();
            e.Exceptions.Add(exception);
            return new ConvertResult(false, null, e);
        }
    }
}