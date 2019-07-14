using System;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换器设置参数
    /// </summary>
    public sealed class ConvertSettings
    {
        /// <summary>
        /// 服务提供程序
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 全局设置
        /// </summary>
        public static ConvertSettings Global { get; private set; }

        public static void Injection(IServiceProvider provider) => 
            Global = new ConvertSettings(provider);

        public ConvertSettings()
            : this(Global)
        {

        }

        public ConvertSettings(ConvertSettings settings = null)
        {
            ServiceProvider = settings?.ServiceProvider;
        }

        public ConvertSettings(IServiceProvider provider = null)
            : this(Global) =>
            ServiceProvider = provider ?? Global?.ServiceProvider;
    }
}
