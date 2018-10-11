using System;

namespace blqw
{
    /// <summary>
    /// 转换结果
    /// </summary>
    public struct ConvertResult
    {
        /// <summary>
        /// 转换成功
        /// </summary>
        /// <param name="value">返回值</param>
        public ConvertResult(object value) : this(true, value, null) { }

        /// <summary>
        /// 转换成功
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="value">返回值</param>
        /// <param name="ex">错误对象</param>
        internal ConvertResult(bool success, object value, ConvertException ex)
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
        public object OutputValue { get; }
        /// <summary>
        /// 如果失败,则返回异常
        /// </summary>
        /// <returns></returns>
        internal ConvertException Exception { get; }

        public void ThrowIfExceptional() => Exception?.TryThrow();

        #region 隐式转换
        public static implicit operator ConvertResult(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var e = new ConvertException(exception);
            return new ConvertResult(false, null, e);
        }

        public static implicit operator ConvertResult(ConvertException exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            return new ConvertResult(false, null, exception);
        }


        public static ConvertResult operator &(ConvertException ex, ConvertResult result)
        {
            if (ex != null && !result.Success)
            {
                result.Exception.AddException(ex);
            }
            return result;
        }
        #endregion

    }
}