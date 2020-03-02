using blqw.Kanai.Convertors;
using blqw.Kanai.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace blqw.Kanai.Extensions
{
    static class ExceptionExtensions
    {
        public static Exception Fail<T>(this IConvertor<T> convertor, object origin, ConvertContext context, ICollection<Exception> exceptions = null)
        {
            if (convertor == null)
            {
                throw new ArgumentNullException(nameof(convertor));
            }

            var text = (origin as IConvertible)?.ToString(null)
                       ?? (origin as IFormattable)?.ToString(null, null);
            var outputTypeName = (convertor as BaseConvertor<T>)?.TypeFriendlyName ?? typeof(T).GetFriendlyName();
            var rs = context.ResourceStrings ?? ResourceStringManager.ZH_CN;
            if (origin == null)
            {
                text = string.Format(rs.CANT_CONVERT, "null", outputTypeName);
            }
            else if (text == null)
            {
                text = string.Format(rs.CANT_CONVERT, origin.GetType().GetFriendlyName(), outputTypeName);
            }
            else
            {
                text = string.Format(rs.VALUE_CANT_CONVERT, text, origin.GetType().GetFriendlyName(), outputTypeName);
            }

            if (exceptions == null)
            {
                return new InvalidCastException(string.Format(rs.CONVERTOR_CAST_FAIL, convertor.GetType().GetFriendlyName(), text));
            }
            else if (exceptions.Count == 1)
            {
                return exceptions.Single();
            }
            else
            {
                return new ConvertException(string.Format(rs.CONVERTOR_CAST_FAIL, convertor.GetType().GetFriendlyName(), text), exceptions);
            }
        }

        public static Exception Error<T>(this IConvertor<T> convertor, Exception ex, ConvertContext context)
        {
            if (convertor == null)
            {
                throw new ArgumentNullException(nameof(convertor));
            }
            ex = ex.GetRealException();
            var rs = context.ResourceStrings ?? ResourceStringManager.ZH_CN;

            return new InvalidCastException(string.Format(rs.CONVERTOR_CAST_FAIL, convertor.GetType().GetFriendlyName(), ex.Message), ex);
        }

        public static OverflowException Overflow<T>(this IConvertor<T> convertor, string message, ConvertContext context)
        {
            if (convertor == null)
            {
                throw new ArgumentNullException(nameof(convertor));
            }

            var rs = context.ResourceStrings ?? ResourceStringManager.ZH_CN;
            return new OverflowException(string.Format(rs.VALUE_OVERFLOW, convertor.GetType().GetFriendlyName(), message));
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
