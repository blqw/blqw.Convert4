using blqw.Kanai.Core;
using blqw.Kanai.Interface;
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

        internal static ConvertSettings Default { get; } = new ConvertSettings((IServiceProvider)new SimpleServiceCollection().AddKanai());

        public ConvertSettings(IServiceProvider serviceProvider = null)
        {
            ServiceProvider = serviceProvider ?? Global?.ServiceProvider ?? Default.ServiceProvider;
            Encoding = serviceProvider?.GetService<Encoding>() ?? Encoding.UTF8;
            CultureInfo = serviceProvider?.GetService<CultureInfo>() ?? CultureInfo.CurrentCulture;
            Translators = serviceProvider?.GetService<IEnumerable<ITranslator>>();
            NumberFormatInfo = serviceProvider?.GetService<NumberFormatInfo>() ?? NumberFormatInfo.CurrentInfo;
            FormatProvider = serviceProvider?.GetService<IFormatProvider>();
            StringSerializer = serviceProvider?.GetService<IStringSerializer>();
            ResourceStrings = serviceProvider?.GetService<ResourceStrings>() ?? ResourceStringManager.ZH_CN;
            ConvertorSelector = serviceProvider?.GetService<IConvertorSelector>()
                                ?? new ConvertorSelector(serviceProvider?.GetServices<IConvertorFactory>());
        }

        public ConvertSettings Option(Action<ConvertSettings> option)
        {
            option(this);
            return this;
        }
        /// <summary>
        /// 服务提供程序
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

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
        public StringSeparator StringSeparators { get; set; } = ",; \n\t|，；";
        /// <summary>
        /// 字符串分割选项
        /// </summary>
        public StringSplitOptions StringSplitOptions { get; set; } = StringSplitOptions.RemoveEmptyEntries;


        /// <summary>
        /// 时间类型格式化字符串, 默认yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string DateTimeFormatString { get; set; }


        public void SetFormatString(Type type, string value)
        {
            if (type == typeof(byte)) _byteFormatString = value;
            else if (type == typeof(DateTime)) _dateTimeFormatString = value;
            else if (type == typeof(DateTimeOffset)) _dateTimeOffsetFormatString = value;
            else if (type == typeof(decimal)) _decimalFormatString = value;
            else if (type == typeof(double)) _doubleFormatString = value;
            else if (type == typeof(FormattableString)) _formattableStringFormatString = value;
            else if (type == typeof(Guid)) _guidFormatString = value;
            else if (type == typeof(short)) _int16FormatString = value;
            else if (type == typeof(int)) _int32FormatString = value;
            else if (type == typeof(long)) _int64FormatString = value;
            else if (type == typeof(sbyte)) _sByteFormatString = value;
            else if (type == typeof(float)) _singleFormatString = value;
            else if (type == typeof(TimeSpan)) _timeSpanFormatString = value;
            else if (type == typeof(ushort)) _uInt16FormatString = value;
            else if (type == typeof(uint)) _uInt32FormatString = value;
            else if (type == typeof(ulong)) _uInt64FormatString = value;
            else if (type == typeof(System.Numerics.BigInteger)) _bigIntegerFormatString = value;
            else if (type == typeof(System.Numerics.Complex)) _complexFormatString = value;
            else if (_otherFormatStrings == null)
            {
                _otherFormatStrings = new List<(Type, string)> { (type, value) };
            }
            else
            {
                for (var i = 0; i < _otherFormatStrings.Count; i++)
                {
                    var (t, s) = _otherFormatStrings[i];
                    if (t == type)
                    {
                        _otherFormatStrings[i] = (type, value);
                        return;
                    }
                }
                _otherFormatStrings.Add((type, value));
            }

        }

        public void SetFormatString<T>(string value)
            where T : IFormattable
            => SetFormatString(typeof(T), value);

        public string GetFormatString<T>() => GetFormatString(typeof(T));

        public string GetFormatString(Type type)
        {
            if (type == typeof(byte)) return _byteFormatString;
            if (type == typeof(DateTime)) return _dateTimeFormatString;
            if (type == typeof(DateTimeOffset)) return _dateTimeOffsetFormatString;
            if (type == typeof(decimal)) return _decimalFormatString;
            if (type == typeof(double)) return _doubleFormatString;
            if (type == typeof(FormattableString)) return _formattableStringFormatString;
            if (type == typeof(Guid)) return _guidFormatString;
            if (type == typeof(short)) return _int16FormatString;
            if (type == typeof(int)) return _int32FormatString;
            if (type == typeof(long)) return _int64FormatString;
            if (type == typeof(sbyte)) return _sByteFormatString;
            if (type == typeof(float)) return _singleFormatString;
            if (type == typeof(TimeSpan)) return _timeSpanFormatString;
            if (type == typeof(ushort)) return _uInt16FormatString;
            if (type == typeof(uint)) return _uInt32FormatString;
            if (type == typeof(ulong)) return _uInt64FormatString;
            if (type == typeof(System.Numerics.BigInteger)) return _bigIntegerFormatString;
            if (type == typeof(System.Numerics.Complex)) return _complexFormatString;

            if (_otherFormatStrings == null)
            {
                return null;
            }
            for (var i = 0; i < _otherFormatStrings.Count; i++)
            {
                var (t, s) = _otherFormatStrings[i];
                if (t == type)
                {
                    return s;
                }
            }
            return null;
        }

        #region FormatStrings

        private string _byteFormatString = "";
        private string _dateTimeFormatString = "";
        private string _dateTimeOffsetFormatString = "";
        private string _decimalFormatString = "";
        private string _doubleFormatString = "";
        private string _formattableStringFormatString = "";
        private string _guidFormatString = "";
        private string _int16FormatString = "";
        private string _int32FormatString = "";
        private string _int64FormatString = "";
        private string _sByteFormatString = "";
        private string _singleFormatString = "";
        private string _timeSpanFormatString = "";
        private string _uInt16FormatString = "";
        private string _uInt32FormatString = "";
        private string _uInt64FormatString = "";
        private string _bigIntegerFormatString = "";
        private string _complexFormatString = "";
        private List<(Type, string)> _otherFormatStrings;



        #endregion
    }
}
