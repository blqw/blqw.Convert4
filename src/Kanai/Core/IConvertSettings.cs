using blqw.Core;
using blqw.Kanai.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace blqw.Kanai
{
    public interface IConvertSettings
    {
        /// <summary>
        /// 字符集
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// 区域信息
        /// </summary>
        CultureInfo CultureInfo { get; set; }
        /// <summary>
        /// 翻译器
        /// </summary>
        IEnumerable<ITranslator> Translators { get; set; }
        /// <summary>
        /// 数字格式信息
        /// </summary>
        NumberFormatInfo NumberFormatInfo { get; set; }
        /// <summary>
        /// 格式化服务提供程序
        /// </summary>
        IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// 时间类型格式化字符串, 默认yyyy-MM-dd HH:mm:ss
        /// </summary>
        string DateTimeFormatString { get; set; }

        /// <summary>
        /// 字符串序列化组件
        /// </summary>
        IStringSerializer StringSerializer { get; set; }

        /// <summary>
        /// 转换器选择器
        /// </summary>
        IConvertorSelector ConvertorSelector { get; set; }

        /// <summary>
        /// 字符串资源
        /// </summary>
        ResourceStrings ResourceStrings { get; set; }
        /// <summary>
        /// 字符串分隔符
        /// </summary>
        IEnumerable<char> StringSeparators { get; set; }
        /// <summary>
        /// 字符串分割选项
        /// </summary>
        StringSplitOptions StringSplitOptions { get; set; }
    }
}
