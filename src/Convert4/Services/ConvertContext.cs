using blqw.ConvertServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using blqw.DI;

namespace blqw
{
    /// <summary>
    /// 转换操作上下文
    /// </summary>
    public sealed class ConvertContext : IServiceProvider, IDisposable
    {
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
        /// <summary>
        /// 转换设置
        /// </summary>
        private readonly ConvertSettings _settings;

        /// <summary>
        /// 使用 <seealso cref="ConvertConfig.Current"/> 创建上下文并提供服务提供程序
        /// </summary>
        public ConvertContext()
            : this(ConvertSettings.Global)
        {

        }

        /// <summary>
        /// 创建上下文并提供服务提供程序
        /// </summary>
        /// <param name="serviceProvider"> 服务提供程序<para />
        /// 当 <paramref name="serviceProvider"/> 为 <see cref="null"/> 时, 使用 <seealso cref="ConvertConfig.Current"/> <para/>
        /// 当 <paramref name="serviceProvider"/> 为 <seealso cref="AggregateServicesProvider"/> 时, 不做处理 <para/>
        /// 否则组合 <paramref name="serviceProvider"/> 和 <seealso cref="ConvertConfig.Current"/>
        /// </param>
        public ConvertContext(ConvertSettings settings)
        {
            if (settings == null)
            {
                settings = ConvertSettings.Global;
            }
            var provider = settings.ServiceProvider as AggregateServicesProvider
                            ?? settings.ServiceProvider.Aggregate(ConvertSettings.Global.ServiceProvider);

            _serviceScope = provider.CreateScope();
            _serviceProvider = _serviceScope.ServiceProvider;
            _convertorSelector = _serviceProvider.GetService<IConvertorSelector>();
            _settings = settings;
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
        public Exception Exception => Error.Exception;

        private ConvertError _error;
        internal ConvertError Error => _error ?? (_error = new ConvertError(null));

        public void ClearException() => _error?.Clear();

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

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">转换后的类型</typeparam>
        /// <param name="input">待转换的实例</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/>为静态类型</exception>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/>为泛型定义类型</exception>
        /// <exception cref="EntryPointNotFoundException">找不到<typeparamref name="T"/>类型的转换器</exception>
        public ConvertResult<T> Convert<T>(object input)
        {
            var outputType = typeof(T);
            if (outputType.IsGenericTypeDefinition)
            {
                return new NotSupportedException(this.Localize($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                return new NotSupportedException(this.Localize($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            var conv = this.GetConvertor<T>();
            if (conv == null)
            {
                return new EntryPointNotFoundException(this.Localize($"未找到适合的转换器"));
            }
            return conv.ChangeType(this, input);
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="outputType">转换后的类型</param>
        /// <param name="input">待转换的实例</param>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/>为静态类型</exception>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/>为泛型定义类型</exception>
        /// <exception cref="EntryPointNotFoundException">找不到<typeparamref name="T"/>类型的转换器</exception>
        /// <returns></returns>
        public ConvertResult Convert(Type outputType, object input)
        {
            if (outputType.IsGenericTypeDefinition)
            {
                return new NotSupportedException(this.Localize($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                return new NotSupportedException(this.Localize($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            var conv = this.GetConvertor(outputType);
            if (conv == null)
            {
                return new EntryPointNotFoundException(this.Localize($"未找到适合的转换器"));
            }
            return conv.ChangeType(this, input);
        }


        internal Exception GetInvalidCastException(object value, string outputTypeName)
        {
            var text = (value as IConvertible)?.ToString(null)
                       ?? (value as IFormattable)?.ToString(null, null);

            if (value == null)
            {
                text = Localize($"`null` {"无法转换为"} {outputTypeName:!}");
            }
            else if (text == null)
            {
                text = Localize($"`{value.GetType().GetFriendlyName():!}` {"无法转换为"} {outputTypeName:!}");
            }
            else
            {
                text = Localize($"{"值"}:`{text:!}`({value.GetType().GetFriendlyName():!}) {"无法转换为"}; {outputTypeName:!}");
            }
            return new InvalidCastException(text);
        }


        /// <summary>
        /// 返回转换无效的异常
        /// </summary>
        /// <param name="value">待转换的值</param>
        /// <param name="outputTypeName">转换后的类型</param>
        /// <returns></returns>
        public void InvalidCastException(object value, string outputTypeName) =>
            Error.AddException(GetInvalidCastException(value, outputTypeName));


        /// <summary>
        /// 返回转换无效的异常
        /// </summary>
        /// <param name="str">格式化消息文本</param>
        /// <returns></returns>
        public void InvalidCastException(FormattableString str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            Error.AddException(new InvalidCastException(Localize(str)));
        }

        public void InvalidOperationException(FormattableString str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            Error.AddException(new InvalidOperationException(Localize(str)));
        }

        /// <summary>
        /// 值超过限制
        /// </summary>
        public void OverflowException(string value) =>
          Error.AddException(new OverflowException(Localize($"{"值超过限制"}:{value:!}")));

        /// <summary>
        /// 本地化字符串
        /// </summary>
        /// <param name="str">格式化消息文本</param>
        /// <returns></returns>
        public string Localize(FormattableString str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            return SR.GetString(this.GetCultureInfo(), str);
        }

        public object CreateInstance<defaultT>(Type outputType)
            where defaultT : new() =>
            outputType.IsAssignableFrom(typeof(defaultT)) ? new defaultT() : this.CreateInstance(outputType);
    }
}