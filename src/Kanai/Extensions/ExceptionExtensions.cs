using blqw.Kanai.Convertors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace blqw.Kanai.Extensions
{
    static class ExceptionExtensions
    {
        public static Exception Fail<T>(this IConvertor<T> convertor, object origin, CultureInfo cultureInfo, ICollection<Exception> exceptions = null)
        {
            if (convertor == null)
            {
                throw new ArgumentNullException(nameof(convertor));
            }

            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            var text = (origin as IConvertible)?.ToString(null)
                       ?? (origin as IFormattable)?.ToString(null, null);
            var outputTypeName = (convertor as BaseConvertor<T>)?.TypeFriendlyName ?? typeof(T).GetFriendlyName();
            if (origin == null)
            {
                text = SR.CANT_CONVERT.Localize(cultureInfo, "null", outputTypeName);
            }
            else if (text == null)
            {
                text = SR.CANT_CONVERT.Localize(cultureInfo, origin.GetType().GetFriendlyName(), outputTypeName);
            }
            else
            {
                text = SR.VALUE_CANT_CONVERT.Localize(cultureInfo, text, origin.GetType().GetFriendlyName(), outputTypeName);
            }

            if (exceptions == null)
            {
                return new InvalidCastException($"{SR.CONVERTOR_FAIL.Localize(cultureInfo, convertor.GetType().GetFriendlyName())}: {text}");
            }
            else if (exceptions.Count == 1)
            {
                return exceptions.Single();
            }
            else
            {
                return new ConvertException($"{SR.CONVERTOR_FAIL.Localize(cultureInfo, convertor.GetType().GetFriendlyName())}:{text}", exceptions);
            }
        }

        public static Exception Error<T>(this IConvertor<T> convertor, Exception ex, CultureInfo cultureInfo)
        {
            if (convertor == null)
            {
                throw new ArgumentNullException(nameof(convertor));
            }

            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            return new InvalidCastException($"{SR.CONVERTOR_FAIL.Localize(cultureInfo, convertor.GetType().GetFriendlyName())}: {ex.Message}", ex);
        }



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
