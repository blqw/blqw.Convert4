using System;
using System.Collections.Concurrent;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换操作上下文
    /// </summary>
    public sealed class ConvertContext : IDisposable
    {
        private ConvertSettings _settings;

        public ConvertContext(ConvertSettings settings)
        {
            _settings = settings ?? ConvertSettings.Global;
        }

        public void Dispose()
        {

        }

        public ConvertResult<T> ChangeType<T>(object input)
        {
            var selector = (IConvertorSelector)_settings.ServiceProvider.GetService(typeof(IConvertorSelector));
            var convertor = selector.GetConvertor<T>(this);
            return convertor.ChangeType(this, input);
        }


    }
}