using System;
using System.Reflection;

namespace blqw.Convertors
{
    /// <summary>
    /// 失败的转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FailConvertor<T> : IConvertor<T>
    {
        /// <summary>
        /// 泛型返回值
        /// </summary>
        private ConvertResult<T> _genericResult;
        /// <summary>
        /// 一般返回值
        /// </summary>
        private ConvertResult _result;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private FailConvertor() { }

        /// <summary>
        /// 直接返回一个转换失败的结果
        /// </summary>
        /// <param name="context"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public ConvertResult<T> ChangeType(ConvertContext context, object input) => _genericResult;
        /// <summary>
        /// 返回一个指定类型的失败转换器
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        public IConvertor<T1> GetConvertor<T1>() => new FailConvertor<T1>() { _genericResult = _result, _result = _result };
        /// <summary>
        ///
        /// </summary>
        public Type OutputType => typeof(T);

        public uint Priority => 0;

        ConvertResult IConvertor.ChangeType(ConvertContext context, object input) => _result;

        public IConvertor GetConvertor(Type outputType) =>
            (IConvertor)typeof(IConvertor<T>).GetMethod("GetConvertor", BindingFlags.DeclaredOnly)
                                             .MakeGenericMethod(new[] { outputType })
                                             .Invoke(this, Array.Empty<object>());

        public static implicit operator FailConvertor<T>(Exception ex) =>
            ex == null ? null : new FailConvertor<T>() { _genericResult = ex, _result = ex };
    }
}
