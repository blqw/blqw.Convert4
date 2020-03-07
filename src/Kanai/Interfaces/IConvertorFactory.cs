using System;

namespace blqw.Kanai.Interface
{
    public interface IConvertorFactory
    {
        IServiceProvider ServiceProvider { get; }
        IConvertor<T> Build<T>();
        bool CanBuild<T>();
    }
}
