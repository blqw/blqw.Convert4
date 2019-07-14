using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace blqw.Kanai.Extensions
{
    static class ExceptionExtensions
    {
        public static Exception GetRealException(this Exception exception)
        {
            if (exception == null)
            {
                return null;
            }
            var baseException = exception.GetBaseException();
            if (baseException != null && baseException != exception)
            {
                return GetRealException(baseException);
            }

            switch (exception)
            {
                case TargetInvocationException e:
                    return GetRealException(e.InnerException);
                default:
                    return exception;
            }

        }
    }
}
