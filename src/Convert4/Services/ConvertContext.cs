using blqw.ConvertServices;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace blqw
{
    /// <summary>
    /// 转换操作上下文
    /// </summary>
    public sealed class ConvertContext : IServiceProvider, IDisposable
    {
        public static readonly ConvertContext None = new ConvertContext();
        /// <summary>
        /// 服务提供程序
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 服务范围
        /// </summary>
        private readonly IServiceScope _serviceScope;
        /// <summary>
        /// 转换器选择器
        /// </summary>
        private readonly IConvertorSelector _convertorSelector;
        private readonly ConvertSettings _settings;


        /// <summary>
        /// 使用 <seealso cref="Startup.ServiceProvider"/> 创建上下文并提供服务提供程序
        /// </summary>
        public ConvertContext()
            : this(null, null)
        {
        }

        /// <summary>
        /// 创建上下文并提供服务提供程序
        /// </summary>
        /// <param name="serviceProvider"> 服务提供程序<para />
        /// 当 <paramref name="serviceProvider"/> 为 <see cref="null"/> 时, 使用 <seealso cref="Startup.ServiceProvider"/> <para/>
        /// 当 <paramref name="serviceProvider"/> 为 <seealso cref="AggregateServicesProvider"/> 时, 不做处理 <para/>
        /// 否则组合 <paramref name="serviceProvider"/> 和 <seealso cref="Startup.ServiceProvider"/>
        /// </param>
        public ConvertContext(ConvertSettings settings)
            : this(settings, null)
        {
        }

        /// <summary>
        /// 创建上下文并提供服务提供程序
        /// </summary>
        /// <param name="serviceProvider"> 服务提供程序<para />
        /// 当 <paramref name="serviceProvider"/> 为 <see cref="null"/> 时, 使用 <seealso cref="Startup.ServiceProvider"/> <para/>
        /// 当 <paramref name="serviceProvider"/> 为 <seealso cref="AggregateServicesProvider"/> 时, 不做处理 <para/>
        /// 否则组合 <paramref name="serviceProvider"/> 和 <seealso cref="Startup.ServiceProvider"/>
        /// </param>
        public ConvertContext(ConvertSettings settings, IServiceProvider serviceProvider)
        {
            var provider = serviceProvider == null
                                ? (IServiceProvider)Startup.ServiceProvider
                                : serviceProvider as AggregateServicesProvider
                                ?? new AggregateServicesProvider(serviceProvider, Startup.ServiceProvider);
            _serviceScope = provider.CreateScope();
            _serviceProvider = _serviceScope.ServiceProvider;
            _convertorSelector = _serviceProvider.GetService<IConvertorSelector>();
            _settings = settings;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">转换后的类型</typeparam>
        /// <param name="input">待转换的实例</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"><typeparamref name="T"/>为静态类型</exception>
        /// <exception cref="ArgumentOutOfRangeException"><typeparamref name="T"/>为泛型定义类型</exception>
        /// <exception cref="EntryPointNotFoundException">找不到<typeparamref name="T"/>类型的转换器</exception>
        public ConvertResult<T> ChangeType<T>(object input)
        {
            var outputType = typeof(T);
            if (outputType.IsGenericTypeDefinition)
            {
                return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            var conv = GetConvertor<T>();
            if (conv == null)
            {
                return new EntryPointNotFoundException(SR.GetString($"转换器未找到"));
            }
            return conv.ChangeType(this, input);
        }

        public ConvertResult ChangeType(Type outputType, object input)
        {
            if (outputType.IsGenericTypeDefinition)
            {
                return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                return new ArgumentOutOfRangeException(SR.GetString($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            var conv = GetConvertor(outputType);
            if (conv == null)
            {
                return new EntryPointNotFoundException(SR.GetString($"转换器未找到"));
            }
            return conv.ChangeType(this, input);
        }

        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IConvertor<T> GetConvertor<T>() => _convertorSelector.Get<T>(this);

        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IConvertor GetConvertor(Type outputType) => _convertorSelector.Get(outputType, this);

        /// <summary>
        /// 施放资源
        /// </summary>
        public void Dispose() => _serviceScope?.Dispose();

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 返回转换无效的异常
        /// </summary>
        /// <param name="value">待转换的值</param>
        /// <param name="outputTypeName">转换后的类型</param>
        /// <returns></returns>
        internal Exception InvalidCastException(object value, string outputTypeName)
        {
            var text = (value is DBNull ? "`DBNull`" : null)
                       ?? (value as IConvertible)?.ToString(null)
                       ?? (value as IFormattable)?.ToString(null, null)
                       ?? (value == null ? "`null`" : null);

            if (text == null)
            {
                return Exception = InvalidCastException($"`{value.GetType().GetFriendlyName():!}` {"无法转换为"} {outputTypeName:!}");
            }
            return Exception = InvalidCastException($"{"值"}:`{value.ToString():!}`({value.GetType().GetFriendlyName():!}) {"无法转换为"}; {outputTypeName:!}");
        }

        /// <summary>
        /// 返回转换无效的异常
        /// </summary>
        /// <param name="str">格式化消息文本</param>
        /// <returns></returns>
        internal Exception InvalidCastException(FormattableString str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            return Exception = new InvalidCastException(SR.GetString(this.GetCultureInfo(), str));
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ConvertSettings))
            {
                return _settings;
            }
            return _settings?.GetService(serviceType) ?? _serviceProvider?.GetService(serviceType);
        }
    }
}