using blqw.Kanai.Extensions;
using blqw.Kanai.Interface;
using blqw.Kanai.Interface.From;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace blqw.Kanai.Convertors
{
    class EnumConvertorFactory : IConvertorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EnumConvertorFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public IConvertor<T> Build<T>()
        {
            if (typeof(T).IsEnum)
            {
                return (IConvertor<T>)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(InnerConvertor<>).MakeGenericType(typeof(T)));
            }
            return null;
        }
        public bool CanBuild<T>() => typeof(T).IsEnum;

        class InnerConvertor<T> : BaseConvertor<T>,
                                  IFrom<string, T>,
                                  IFrom<IConvertible, T>
        where T : struct
        {
            public ConvertResult<T> From(ConvertContext context, string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return default;
                }
                T result;
                try
                {
                    result = (T)Enum.Parse(OutputType, input, true);
                }
                catch (Exception ex)
                {
                    return ex;
                }

                if (result.Equals(default(T)))
                {
                    return default;
                }
                if (Enum.IsDefined(OutputType, result))
                {
                    return result;
                }
                if (Attribute.IsDefined(OutputType, typeof(FlagsAttribute)))
                {
                    if (result.ToString().IndexOf(',') >= 0)
                    {
                        return result;
                    }
                }
                return this.Fail(context, input);
            }

            public ConvertResult<T> From(ConvertContext context, IConvertible input)
            {
                T result;
                switch (input.GetTypeCode())
                {
                    case TypeCode.Empty:
                    case TypeCode.DBNull:
                    case TypeCode.DateTime:
                    case TypeCode.Boolean:
                        return default;
                    case TypeCode.Decimal:
                    case TypeCode.Char:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                    case TypeCode.Double:
                    case TypeCode.Single:
                        result = (T)Enum.ToObject(OutputType, input.ToInt64(null));
                        break;
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        result = (T)Enum.ToObject(OutputType, input.ToUInt64(null));
                        break;
                    case TypeCode.String:
                        return From(context, input.ToString(null));
                    case TypeCode.Object:
                    default:
                        return default;
                }
                if (result.Equals(default(T)))
                {
                    return result;
                }
                if (Enum.IsDefined(OutputType, result))
                {
                    return result;
                }
                if (Attribute.IsDefined(OutputType, typeof(FlagsAttribute)))
                {
                    if (result.ToString().IndexOf(',') >= 0)
                    {
                        return result;
                    }
                }
                return this.Fail(context, input);
            }
        }

    }
}
