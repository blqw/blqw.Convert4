using blqw.ConvertServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class EnumConvertor : BaseConvertor<Enum>
    {
        public override Type OutputType => typeof(Enum);

        public override IConvertor GetConvertor(Type outputType)
        {
            if (outputType == null)
            {
                throw new ArgumentNullException(nameof(outputType));
            }
            if (outputType.IsEnum == false)
            {
                throw new ArgumentOutOfRangeException(nameof(outputType), $"类型{outputType.GetFriendlyName()}必须是枚举");
            }

            return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<>).MakeGenericType(outputType), this);
        }

        class InnerConvertor<T> : BaseConvertor<T>,
                                  IFrom<string, T>,
                                  IFrom<IConvertible, T>
        where T : struct
        {
            private readonly EnumConvertor _parent;

            public InnerConvertor(EnumConvertor parent) => _parent = parent;

            public override IConvertor GetConvertor(Type outputType) => _parent.GetConvertor(outputType);

            public T From(ConvertContext context, string input)
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
                    context.Error.AddException(ex);
                    return default;
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
                context.OverflowException($"{result:!} {"不是有效的枚举值"}");
                return default;
            }

            public T From(ConvertContext context, IConvertible input)
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
                context.OverflowException($"{result:!} {"不是有效的枚举值"}");
                return default;
            }
        }
    }
}
