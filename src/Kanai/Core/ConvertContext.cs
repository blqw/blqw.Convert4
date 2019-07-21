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
            var outputType = typeof(T);
            if (outputType.IsGenericTypeDefinition)
            {
                //return new NotSupportedException(Localize($"{"无法为"}{"泛型定义类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                //return new NotSupportedException(Localize($"{"无法为"}{"静态类型"}`{outputType.GetFriendlyName():!}`{"提供转换器"}"));
            }

            var selector = (IConvertorSelector)_settings.ServiceProvider.GetService(typeof(IConvertorSelector));
            var convertor = selector.GetConvertor<T>(this);
            if (convertor == null)
            {
                //return new EntryPointNotFoundException(Localize($"未找到适合的转换器"));
            }
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