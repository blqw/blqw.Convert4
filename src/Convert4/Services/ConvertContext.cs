using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System;
using blqw.Services;

namespace blqw
{
    /// <summary>
    /// 转换操作上下文
    /// </summary>
    public class ConvertContext : IServiceProvider, IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IServiceScope _serviceScope;
        private readonly IConvertorSelector _convertorSelector;

        public ConvertContext() : this(null)
        {
        }

        public ConvertContext(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? Startup.ServiceProvider;
            _serviceScope = _serviceProvider.CreateScope();
            _convertorSelector = _serviceProvider.GetRequiredService<IConvertorSelector>();
        }

        public ConvertResult<T> ChangeType<T>(ConvertContext context, object input)
        {
            var conv = context.GetConvertor<T>();
            if(conv == null)
            {
                return new EntryPointNotFoundException(SR.GetString("转换器未找到"));
            }
            return conv.ChangeType(context, input);
        }
        public IConvertor<T> GetConvertor<T>() => _convertorSelector.Get<T>(this);

        public object GetService(Type serviceType) => null;

        public T GetService<T>() => (T)GetService(typeof(T));

        public void Dispose() => _serviceScope?.Dispose();

        public Exception Exception { get; set; }

        public Exception Error(params string[] messages) =>
            Exception = new InvalidCastException(SR.Concat(this.GetCultureInfo(), messages));

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
            return Exception = new InvalidCastException(SR.GetString(this.GetCultureInfo(), $"`{value.GetType().GetFriendlyName():!}` {"值"}: {"无法转换为"}; {outputTypeName:!}"));
        }
    }
}
