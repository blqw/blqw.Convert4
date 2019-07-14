using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Kanai
{
    public interface IConvertorFactory
    {
        IConvertor<T> Build<T>();

        bool CanBuild<T>();
    }
}
