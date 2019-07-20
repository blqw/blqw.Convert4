using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换操作上下文
    /// </summary>
    public sealed class ConvertContext : IDisposable
    {
        private ConvertSettings _settings;

        public CultureInfo CultureInfo { get; }
        public IEnumerable<ITranslator> Translators { get; }

        public ConvertContext(ConvertSettings settings)
        {
            _settings = settings ?? ConvertSettings.Global;
            Translators = _settings.GetServices<ITranslator>();
            CultureInfo = _settings.GetService<CultureInfo>() ?? CultureInfo.CurrentCulture;
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

        public IEnumerable<object> Translate(object input)
        {
            var type = input.GetType(); 
            if (Translators != null)
            {
                foreach (var translator in Translators)
                {
                    if (translator.InputType.IsAssignableFrom(type))
                    {
                        yield return translator.Translate(input);
                    }
                }
            }
        }
    }
}