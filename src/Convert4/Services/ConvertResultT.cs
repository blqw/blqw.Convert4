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
            Error = null;
        }

        private ConvertResult(bool success, T value, ConvertError ex)
        {
            Success = success;
            OutputValue = value;
            Error = ex;
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
        public ConvertError Error { get; }
        /// <summary>
        /// 如果有异常则抛出异常
        /// </summary>
        /// <exception cref="AggregateException"> 发生一个或多个转换错误问题 </exception>
        public void ThrowIfExceptional() => Error?.TryThrow();

        public static implicit operator ConvertResult(ConvertResult<T> value) => new ConvertResult(value.Success, value.OutputValue, value.Error);
        public static implicit operator ConvertResult<T>(ConvertResult value) => new ConvertResult<T>(value.Success, (T)(value.OutputValue ?? default(T)), value.Error);

        public static implicit operator ConvertResult<T>(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            var e = new ConvertError();
            e.Exceptions.Add(exception);
            return new ConvertResult(false, null, e);
        }
    }
}