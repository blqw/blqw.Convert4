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

        private ConvertResult(bool success, object value, ConvertException ex)
        {
            Success = success;
            OutputValue = value;
            Exception = ex;
        }

        /// <summary>
        /// 转换中出现异常
        /// </summary>
        /// <param name="ex"></param>
        public static ConvertResult Error(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            var e = new ConvertException();
            e.Exceptions.Add(ex);
            return new ConvertResult(false, null, e);
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

    }
}