using System.Runtime.Serialization;
using blqw.Services;
using System;
using System.Globalization;
using System.Data;
using System.Collections.Specialized;
using System.Collections;

namespace blqw.Convertors
{
    /// <summary>
    /// 转换器基类
    /// </summary>
    /// <typeparam name="T">输出类型</typeparam>
    public abstract class BaseConvertor<T> : IConvertor<T>
    {
        protected virtual T CreateOutput() =>
            Activator.CreateInstance<T>();

        /// <summary>
        /// 输出类型
        /// </summary>
        public virtual Type OutputType { get; } = typeof(T);

        /// <summary>
        /// <seealso cref="OutputType"/>.FullName
        /// </summary>
        public string TypeName { get; } = typeof(T).FullName;

        /// <summary>
        /// <seealso cref="OutputType"/>的友好名称
        /// </summary>
        public string TypeFriendlyName { get; } = typeof(T).GetFriendlyName();

        ConvertResult IConvertor.ChangeType(ConvertContext context, object input)
        {
            context.Exception = null;
            if (input == null)
            {
                if (this is IFromNull<T> conv)
                {
                    var result = conv.FromNull(context);
                    if (context.Exception != null)
                    {
                        return context.Exception;
                    }
                    return new ConvertResult<T>(result);
                }
                return context.Exception = new InvalidCastException(SR.GetString($"`null`{"无法转换为"} {TypeFriendlyName:!}"));
            }
            else if (input is IConvertible v0)
            {
                switch (v0.GetTypeCode())
                {
                    case TypeCode.DBNull:
                        if (this is IFromNull<T> conv)
                        {
                            var result = conv.FromDBNull(context);
                            if (context.Exception != null)
                            {
                                return context.Exception;
                            }
                            return new ConvertResult<T>(result);
                        }
                        return context.Exception = new InvalidCastException(SR.GetString($"`DBNull`{"无法转换为"} {TypeFriendlyName:!}"));
                    case TypeCode.Boolean:
                        return ChangeTypeImpl(context, v0.ToBoolean(context.GetCultureInfo()));
                    case TypeCode.Byte:
                        return ChangeTypeImpl(context, v0.ToByte(context.GetCultureInfo()));
                    case TypeCode.Char:
                        return ChangeTypeImpl(context, v0.ToChar(context.GetCultureInfo()));
                    case TypeCode.DateTime:
                        return ChangeTypeImpl(context, v0.ToDateTime(context.GetCultureInfo()));
                    case TypeCode.Decimal:
                        return ChangeTypeImpl(context, v0.ToDecimal(context.GetCultureInfo()));
                    case TypeCode.Double:
                        return ChangeTypeImpl(context, v0.ToDouble(context.GetCultureInfo()));
                    case TypeCode.Int16:
                        return ChangeTypeImpl(context, v0.ToInt16(context.GetCultureInfo()));
                    case TypeCode.Int32:
                        return ChangeTypeImpl(context, v0.ToInt32(context.GetCultureInfo()));
                    case TypeCode.Int64:
                        return ChangeTypeImpl(context, v0.ToInt64(context.GetCultureInfo()));
                    case TypeCode.SByte:
                        return ChangeTypeImpl(context, v0.ToSByte(context.GetCultureInfo()));
                    case TypeCode.Single:
                        return ChangeTypeImpl(context, v0.ToSingle(context.GetCultureInfo()));
                    case TypeCode.String:
                        return ChangeTypeImpl(context, v0.ToString(context.GetCultureInfo()));
                    case TypeCode.UInt16:
                        return ChangeTypeImpl(context, v0.ToUInt16(context.GetCultureInfo()));
                    case TypeCode.UInt32:
                        return ChangeTypeImpl(context, v0.ToUInt32(context.GetCultureInfo()));
                    case TypeCode.UInt64:
                        return ChangeTypeImpl(context, v0.ToUInt64(context.GetCultureInfo()));
                    default:
                        break;
                }
            }
            else if (input is Guid v1)
            {
                return ChangeTypeImpl(context, v1);
            }
            else if (input is TimeSpan v2)
            {
                return ChangeTypeImpl(context, v2);
            }
            else if (input is Uri v3)
            {
                return ChangeTypeImpl(context, v3);
            }
            else if (input is Type v4)
            {
                return ChangeTypeImpl(context, v4);
            }
            else if (input is IntPtr v5)
            {
                return ChangeTypeImpl(context, v5);
            }
            else if (input is UIntPtr v6)
            {
                return ChangeTypeImpl(context, v6);
            }
            else if (input is DataRow v7)
            {
                return ChangeTypeImpl(context, v7);
            }
            else if (input is DataTable v8)
            {
                return ChangeTypeImpl(context, v8);
            }
            else if (input is NameObjectCollectionBase v9)
            {
                return ChangeTypeImpl(context, v9);
            }
            else if (input is StringDictionary v10)
            {
                return ChangeTypeImpl(context, v1);
            }
            else if (input is Array v11)
            {
                return ChangeTypeImpl(context, v11);
            }

            if (input is IDictionary v12)
            {
                return ChangeTypeImpl(context, v12);
            }
            else if (input is IList v13)
            {
                return ChangeTypeImpl(context, v13);
            }
            else if (input is ValueType v14)
            {
                return ChangeTypeImpl(context, v14);
            }
            return ChangeTypeImpl(context, input);
        }

        private delegate bool ConvertHandler<TInput>(ConvertContext context, TInput input, out T result);

        ConvertResult ChangeTypeImpl<TInput>(ConvertContext context, TInput input)
        {
            try
            {

                if (this is IFrom<T, TInput> conv)
                {
                    var result = conv.From(context, input);
                    if (context.Exception != null)
                    {
                        return context.Exception;
                    }
                    return new ConvertResult(result);
                }
                if (input is IObjectReference refObj)
                {
                    var newObj = refObj.GetRealObject(new StreamingContext(StreamingContextStates.Clone, context));
                    if (!ReferenceEquals(newObj, refObj))
                    {
                        return ((IConvertor)this).ChangeType(context, newObj);
                    }
                }
            }
            catch (Exception e)
            {
                return context.Exception = e;
            }

            return context.Error(input, TypeFriendlyName);
        }

        /// <summary>
        /// 获取子转换器
        /// </summary>
        /// <typeparam name="T1">子转换器输出类型</typeparam>
        /// <returns></returns>
        public virtual IConvertor<T1> GetConvertor<T1>()
        {
            var conv = GetConvertor(typeof(T1));
            if (conv == null)
            {
                return null;
            }
            return conv as IConvertor<T1> ?? new GenericProxyConvertor<T1>(conv);
        }

        /// <summary>
        /// 转换转换方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual ConvertResult<T> ChangeType(ConvertContext context, object input) =>
            ((IConvertor)this).ChangeType(context, input);

        /// <summary>
        /// 获取子转换器
        /// </summary>
        /// <param name="outputType">子转换器输出类型</param>
        /// <returns></returns>
        public virtual IConvertor GetConvertor(Type outputType) => throw new NotImplementedException(SR.GetString("无法获取子转换器"));

        public virtual uint Priority { get; } = 0;
    }
}