﻿using blqw.Kanai.Core;
using blqw.Kanai.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换操作上下文
    /// </summary>
    public struct ConvertContext
    {
        public Type OutputType { get; }

        public ConvertSettings Settings { get; set; }

        public ConvertContext(Type outputType, ConvertSettings settings)
        {
            OutputType = outputType ?? throw new ArgumentNullException(nameof(outputType));
            Settings = settings ?? ConvertSettings.Global ?? ConvertSettings.Default;
            ServiceProvider = Settings.ServiceProvider;
            Encoding = Settings.Encoding;
            CultureInfo = Settings.CultureInfo;
            Translators = Settings.Translators;
            NumberFormatInfo = Settings.NumberFormatInfo;
            FormatProvider = Settings.FormatProvider;
            StringSerializer = Settings.StringSerializer;
            DateTimeFormatString = Settings.DateTimeFormatString;
            ConvertorSelector = Settings.ConvertorSelector;
            ResourceStrings = Settings.ResourceStrings ?? ResourceStringManager.ZH_CN;
            StringSeparators = Settings.StringSeparators;
            StringSplitOptions = Settings.StringSplitOptions;
            FormatString = Settings.GetFormatString(outputType);
        }

        public ConvertContext NewContext(Type outputType)
            => new ConvertContext(outputType, Settings);

        /// <summary>
        /// 字符串序列化组件
        /// </summary>
        public IStringSerializer StringSerializer { get; set; }

        /// <summary>
        /// 服务提供程序
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 字符集
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 区域信息
        /// </summary>
        public CultureInfo CultureInfo { get; set; }
        /// <summary>
        /// 翻译器
        /// </summary>
        public IEnumerable<ITranslator> Translators { get; set; }
        /// <summary>
        /// 数字格式信息
        /// </summary>
        public NumberFormatInfo NumberFormatInfo { get; set; }


        /// <summary>
        /// 格式化服务提供程序
        /// </summary>
        public IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// 时间类型格式化字符串, 默认yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string DateTimeFormatString { get; set; }

        public string FormatString { get; set; }

        /// <summary>
        /// 转换器选择器
        /// </summary>
        public IConvertorSelector ConvertorSelector { get; set; }

        /// <summary>
        /// 字符串资源
        /// </summary>
        public ResourceStrings ResourceStrings { get; set; }

        /// <summary>
        /// 字符串分隔符
        /// </summary>
        public StringSeparator StringSeparators { get; set; }

        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IConvertor<T> GetConvertor<T>() => ConvertorSelector?.GetConvertor<T>(this);

        public ConvertResult<T> Convert<T>(object input)
        {
            var outputType = typeof(T);
            if (outputType.IsAbstract && outputType.IsSealed)
            {
                var text = string.Format(ResourceStrings.CANT_BUILD_CONVERTOR_BECAUSE_STATIC_TYPE, outputType.GetFriendlyName());
                return new NotSupportedException(text);
            }
            var conv = GetConvertor<T>();
            if (conv == null)
            {
                var text = string.Format(ResourceStrings.CANT_BUILD_CONVERTOR_BECAUSE_NOTFOUND, outputType.GetFriendlyName());
                return new EntryPointNotFoundException(text);
            }
            if (outputType == OutputType)
            {
                return conv.Convert(this, input);
            }
            return conv.Convert(NewContext(outputType), input);
        }

        public IEnumerable<object> Translate(object input)
        {
            var type = input.GetType();
            if (Settings.Translators != null)
            {
                foreach (var translator in Settings.Translators)
                {
                    if (translator.CanTranslate(type))
                    {
                        yield return translator.Translate(this, input);
                    }
                }
            }
        }

        public StringSplitOptions StringSplitOptions { get; set; }
    }
}
