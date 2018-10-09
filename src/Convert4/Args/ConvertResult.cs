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
        public ConvertResult(object value)
        {
            Success = true;
            OutputValue = value;
            Error = null;
        }
        /// <summary>
        /// 转换成功
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="value">返回值</param>
        /// <param name="ex">错误对象</param>
        internal ConvertResult(bool success, object value, ConvertError ex)
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
        public object OutputValue { get; }
        /// <summary>
        /// 如果失败,则返回异常
        /// </summary>
        /// <returns></returns>
        internal ConvertError Error { get; }

        public void ThrowIfExceptional() => Error?.TryThrow();

        #region 隐式转换
        public static implicit operator ConvertResult(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var e = new ConvertError(exception);
            return new ConvertResult(false, null, e);
        }

        public static implicit operator ConvertResult(ConvertError error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }
            return new ConvertResult(false, null, error.Clone());
        }

        #endregion

    }
}