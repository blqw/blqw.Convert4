using System.Runtime.CompilerServices;
using System;

namespace blqw
{
    /// <summary>
    /// 转换结果
    /// </summary>
    public struct ConvertResult
    {
        public ConvertResult(object value)
        {
            Success = true;
            OutputValue = value;
            Exception = null;
        }

        internal ConvertResult(bool success, object value, ConvertException ex)
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
        public object OutputValue { get; }
        /// <summary>
        /// 如果失败,则返回异常
        /// </summary>
        /// <returns></returns>
        public ConvertException Exception { get; }

        public static implicit operator ConvertResult(Exception exception)
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