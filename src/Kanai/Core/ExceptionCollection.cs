﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace blqw.Kanai
{
    public sealed class ExceptionCollection : List<Exception>
    {
        public static ExceptionCollection operator +(ExceptionCollection ex1, Exception ex2)
        {
            if (ex2 == null)
            {
                return ex1;
            }
            if (ex1 == null)
            {
                return new ExceptionCollection() { ex2 };
            }
            ex1.Add(ex2);
            return ex1;
        }

        public ConvertException ToConvertException(Type outputType, object origin, CultureInfo cultureInfo)
            => new ConvertException(outputType, origin, this, cultureInfo);

        public ConvertException ToConvertException(string outputTypeName, object origin, CultureInfo cultureInfo)
            => new ConvertException(outputTypeName, origin, this, cultureInfo);
    }
}
