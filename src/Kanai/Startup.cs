using blqw;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: AssemblyStartup(typeof(blqw.Kanai.Startup))]
namespace blqw.Kanai
{
    class Startup
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services) => services.AddKanai();

        /// <summary>
        /// 安装服务
        /// </summary>
        public void Configure(IServiceProvider serviceProvider) => ConvertSettings.Injection(serviceProvider);
    }
}
