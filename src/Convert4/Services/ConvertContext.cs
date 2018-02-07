using System.Reflection;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System;
using blqw.Services;
using System.Collections;

namespace blqw
{
    /// <summary>
    /// 转换操作上下文
    /// </summary>
    public class ConvertContext : IServiceProvider, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScope _serviceScope;
        private readonly IConvertorSelector _convertorSelector;


        /// <summary>
        /// 使用 <seealso cref="Startup.ServiceProvider"/> 创建上下文并提供服务提供程序
        /// </summary>
        public ConvertContext()
            : this(null)
        {
        }

        /// <summary>
        /// 创建上下文并提供服务提供程序
        /// </summary>
        /// <param name="serviceProvider"> 服务提供程序<para />
        /// 当 <paramref name="serviceProvider"/> 为 <see cref="null"/> 时, 使用 <seealso cref="Startup.ServiceProvider"/> <para/>
        /// 当 <paramref name="serviceProvider"/> 为 <seealso cref="MultiServicesProvider"/> 时, 不做处理 <para/>
        /// 否则组合 <paramref name="serviceProvider"/> 和 <seealso cref="Startup.ServiceProvider"/>
        /// </param>
        public ConvertContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider == null
                                ? (IServiceProvider)Startup.ServiceProvider
                                : serviceProvider as MultiServicesProvider ?? new MultiServicesProvider(serviceProvider, Startup.ServiceProvider);
            _serviceScope = _serviceProvider.CreateScope();
            _convertorSelector = _serviceProvider.GetRequiredService<IConvertorSelector>();
        }


        public ConvertResult<T> ChangeType<T>(object input)
        {
            var outputType = typeof(T);
            if (outputType != null)
            {
                if (outputType.IsGenericTypeDefinition)
                {
                    return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
                }
                if (outputType.IsAbstract && outputType.IsSealed)
                {
                    return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
                }
            }
            var conv = GetConvertor<T>();
            if (conv == null)
            {
                return new EntryPointNotFoundException(SR.GetString("转换器未找到"));
            }
            return conv.ChangeType(this, input);
        }

        public IConvertor<T> GetConvertor<T>() => _convertorSelector.Get<T>(this);

        public object GetService(Type serviceType) => _serviceProvider.GetService(serviceType);

        public T GetService<T>() => _serviceProvider.GetService<T>();

        public void Dispose() => _serviceScope?.Dispose();

        public Exception Exception { get; set; }

        public Exception Error(string message) =>
            Exception = new InvalidCastException(SR.Concat(this.GetCultureInfo(), message));


        internal Exception Error(object value, string outputTypeName)
        {
            var text = (value is DBNull ? "`DBNull`" : null)
                       ?? (value as IConvertible)?.ToString(null)
                       ?? (value as IFormattable)?.ToString(null, null)
                       ?? (value == null ? "`null`" : null);

            if (text == null)
            {
                return Exception = new InvalidCastException(SR.GetString(this.GetCultureInfo(), $"`{value.GetType().GetFriendlyName():!}` {"无法转换为"} {outputTypeName:!}"));
            }
            return Exception = new InvalidCastException(SR.GetString(this.GetCultureInfo(), $"{"值"}:`{value.ToString():!}`({value.GetType().GetFriendlyName():!}) {"无法转换为"}; {outputTypeName:!}"));
        }
    }
}
