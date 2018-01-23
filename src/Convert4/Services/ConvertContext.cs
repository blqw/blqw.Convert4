using System;

namespace blqw
{
    /// <summary>
    /// 转换操作上下文
    /// </summary>
    public class ConvertContext : IServiceProvider, IDisposable
    {
        public ConvertContext()
        {

        }

        public IConvertor<T> GetConvertor<T>() => throw new NotImplementedException();
        public void Dispose()
        {

        }

        public object GetService(Type serviceType) => throw new NotImplementedException();

        public T GetService<T>() => (T)GetService(typeof(T));
    }
}