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
                            ?? settings.ServiceProvider.Join(ConvertSettings.Global.ServiceProvider);

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





    }
}