using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换结果
    /// </summary>
    [DebuggerDisplay("{Success} : {Success ? (object)OutputValue : Exception.Message}")]
    public struct ConvertResult<T> : IObjectReference
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
        internal ConvertResult(bool success, T value, Exception ex)
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
        public Exception Exception { get; }

        public void ThrowIfExceptional()
        {
            if (Exception != null)
            {
                throw Exception;
            }
        }

        #region 隐式转换

        public static implicit operator ConvertResult<T>(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            return new ConvertResult<T>(false, default, exception);
        }

        object IObjectReference.GetRealObject(StreamingContext context)
        {
            ThrowIfExceptional();
            return OutputValue;
        }

        public static implicit operator ConvertResult<T>(T value) => new ConvertResult<T>(true, value, null);

        #endregion
    }
}