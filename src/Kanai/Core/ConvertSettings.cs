using blqw.Core;
using blqw.Kanai.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换器设置参数
    /// </summary>
    public sealed class ConvertSettings
    {
        /// <summary>
        /// 全局设置
        /// </summary>
        public static ConvertSettings Global { get; internal set; }


        public ConvertSettings(IServiceProvider serviceProvider = null)
        {
            ServiceProvider = serviceProvider ?? Global?.ServiceProvider;
            Encoding = serviceProvider?.GetService<Encoding>() ?? Encoding.UTF8;
            CultureInfo = serviceProvider?.GetService<CultureInfo>() ?? CultureInfo.CurrentCulture;
            Translators = serviceProvider?.GetServices<ITranslator>();
            NumberFormatInfo = serviceProvider?.GetService<NumberFormatInfo>() ?? NumberFormatInfo.CurrentInfo;
            FormatProvider = serviceProvider?.GetService<IFormatProvider>();
            StringSerializer = serviceProvider?.GetService<IStringSerializer>();
            ResourceStrings = serviceProvider?.GetService<ResourceStrings>() ?? ResourceStringManager.ZH_CN;
            ConvertorSelector = new ConvertorSelector(serviceProvider?.GetServices<IConvertorFactory>());
        }

        /// <summary>
        /// 服务提供程序
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 字符集
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// 区域信息
        /// </summary>
        public CultureInfo CultureInfo { get; }
        /// <summary>
        /// 翻译器
        /// </summary>
        public IEnumerable<ITranslator> Translators { get; }
        /// <summary>
        /// 数字格式信息
        /// </summary>
        public NumberFormatInfo NumberFormatInfo { get; }
        /// <summary>
        /// 格式化服务提供程序
        /// </summary>
        public IFormatProvider FormatProvider { get; }

        /// <summary>
        /// 时间类型格式化字符串, 默认yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string DateTimeFormatString { get; set; }

        /// <summary>
        /// 字符串序列化组件
        /// </summary>
        public IStringSerializer StringSerializer { get; set; }

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
        public IEnumerable<char> StringSeparators { get; set; } = ",; \n\t|，；";
    }
}
