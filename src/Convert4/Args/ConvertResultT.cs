using System;
using System.Collections;

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
        public ConvertResult(T value) : this(true, value, null) { }

        /// <summary>
        /// 转换成功
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="value">返回值</param>
        /// <param name="ex">错误对象</param>
        private ConvertResult(bool success, T value, ConvertException ex)
        {
            _fail = !success;
            OutputValue = value;
            Exception = ex;
        }

        private readonly bool _fail;

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool Success => !_fail;
        /// <summary>
        /// 转换后的输出结果
        /// </summary>
        /// <returns></returns>
        public T OutputValue { get; }
        /// <summary>
        /// 如果失败,则返回异常
        /// </summary>
        /// <returns></returns>
        internal ConvertException Exception { get; private set; }
        /// <summary>
        /// 如果有异常则抛出异常
        /// </summary>
        /// <exception cref="AggregateException"> 发生一个或多个转换错误问题 </exception>
        public void ThrowIfExceptional() => Exception?.TryThrow();

        internal ConvertResult<T0> Cast<T0>()
        {
            if (!Success)
            {
                return new ConvertResult<T0>(false, default, Exception);
            }
            if (OutputValue is T0 v)
            {
                return new ConvertResult<T0>(true, v, null);
            }
            throw new InvalidCastException();
        }

        #region 隐式转换
        public static implicit operator ConvertResult<T>(T value) => new ConvertResult<T>(true, value, null);
        public static implicit operator ConvertResult(ConvertResult<T> value) => new ConvertResult(value.Success, value.OutputValue, value.Exception);
        public static implicit operator ConvertResult<T>(ConvertResult value) => new ConvertResult<T>(value.Success, value.OutputValue is T t ? t : default, value.Exception);
        public static implicit operator ConvertResult<T>(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            var e = new ConvertException(exception);
            return new ConvertResult<T>(false, default, e);
        }
        public static implicit operator ConvertResult<T>(ConvertException exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            return new ConvertResult<T>(false, default, exception);
        }

        public static ConvertResult<T> operator &(ConvertException ex, ConvertResult<T> result)
        {
            if (ex != null && !result.Success)
            {
                result.Exception = ex + result.Exception;
            }
            return result;
        }

        #endregion
    }
}