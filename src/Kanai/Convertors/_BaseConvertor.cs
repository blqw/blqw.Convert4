using blqw.Kanai.Extensions;
using blqw.Kanai.Interface;
using blqw.Kanai.Interface.From;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// 转换器基类
    /// </summary>
    /// <typeparam name="T">输出类型</typeparam>
    public abstract partial class BaseConvertor<T> : IConvertor<T>
    {

        /// <summary>
        /// 调用器字典
        /// </summary>
        private readonly Dictionary<Type, InvokeIFormHandler> _invokers;
        private readonly Type[] _invokerTypes;


        protected BaseConvertor(IServiceProvider serviceProvider)
            : this(typeof(T))
        {
            //typeof(T).GetConstructor
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 构造函数, 根据实际类型的 <seealso cref="IFrom{TOutput, TInput}"/> 接口情况, 按照 TInput 类型缓存调用器
        /// </summary>
        //public BaseConvertor()
        //    : this(typeof(T))
        //{
        //}

        protected BaseConvertor(Type outputType)
        {
            _invokers = InitInvokers();
            _invokerTypes = _invokers.Keys.ToArray();
            Array.Sort(_invokerTypes, (a, b) => a.IsAssignableFrom(b) ? 1 : b.IsAssignableFrom(a) ? -1 : 0);
            OutputType = outputType;
            TypeName = OutputType.FullName;
            TypeFriendlyName = OutputType.GetFriendlyName();
        }



        /// <summary>
        /// 输出类型
        /// </summary>
        public virtual Type OutputType { get; }

        /// <summary>
        /// <seealso cref="OutputType"/>.FullName
        /// </summary>
        public virtual string TypeName { get; }

        /// <summary>
        /// <seealso cref="OutputType"/>的友好名称
        /// </summary>
        public virtual string TypeFriendlyName { get; }


        private ConvertResult<T> TryFrom(ConvertContext context, object input, bool translation, ref ExceptionCollection exceptions)
        {
            //空值转换
            if (input == null)
            {
                try
                {
                    if (this is IFromNull<T> conv)
                    {
                        var result = conv.FromNull(context);
                        if (result.Success)
                        {
                            return result;
                        }
                        exceptions += result.Exception;
                    }
                }
                catch (Exception e)
                {
                    exceptions += e;
                }

                return new ConvertResult<T>(false, default, null);

            }

            // 精确匹配

            var invoker0 = GetInvoker(input.GetType());
            if (invoker0 != null)
            {
                var result = invoker0(this, context, input);
                if (result.Success)
                {
                    return result;
                }
                exceptions += result.Exception;
            }


            if (translation)
            {
                foreach (var value in context.Translate(input))
                {
                    if (input != value)
                    {
                        var result = TryFrom(context, value, false, ref exceptions);
                        if (result.Success)
                        {
                            return result;
                        }
                        //如果异常,下面还可以尝试其他方案
                        exceptions += result.Exception;
                    }
                }
            }


            //获取指定输入类型的转换方法调用器
            var invokers = GetInvokers(input.GetType());
            foreach (var invoker in invokers)
            {
                if (invoker == invoker0)
                {
                    continue;
                }
                var result = invoker(this, context, input);
                if (result.Success)
                {
                    return result;
                }
                //如果异常,下面还可以尝试其他方案
                exceptions += result.Exception;
            }

            return new ConvertResult<T>(false, default, null);

        }


        private ConvertResult<T> TryStringSerializer(ConvertContext context, object input, ref ExceptionCollection exceptions)
        {
            //字符串类型的序列化器
            if (input is string str)
            {
                var serializer = context.StringSerializer;
                if (serializer != null)
                {
                    try
                    {
                        return (T)serializer.ToObject(str, typeof(T));
                    }
                    catch (Exception ex)
                    {
                        exceptions += ex;
                    }
                }
            }
            else if (typeof(T) == typeof(string))
            {
                var serializer = context.StringSerializer;
                if (serializer != null)
                {
                    try
                    {
                        return (T)(object)serializer.ToString(input);
                    }
                    catch (Exception ex)
                    {
                        exceptions += ex;
                    }
                }
            }
            return new ConvertResult<T>(false, default, null);
        }
        /// <summary>
        /// 转换转换方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual ConvertResult<T> Convert(ConvertContext context, object input)
        {
            if (input is T t)
            {
                return Ok(t);
            }

            ExceptionCollection exceptions = null;
            var result = TryStringSerializer(context, input, ref exceptions);
            if (result.Success)
            {
                return result;
            }
            result = TryFrom(context, input, true, ref exceptions);
            if (result.Success)
            {
                return result;
            }

            result = TryConvertible(context, input, ref exceptions);
            if (result.Success)
            {
                return result;
            }
            return this.Fail(context, input, exceptions);
        }

        private ConvertResult<T> TryConvertible(ConvertContext context, object input, ref ExceptionCollection exceptions)
        {
            var fail = new ConvertResult<T>(false, default, null);
            // 转为各种基本类型进行转换, 这里也可以使用翻译器来做, 但是考虑值类型装箱拆箱引起的性能问题, 决定写死
            if (input is IConvertible v0)
            {
                switch (v0.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return v0 is bool ? fail : TryFromGeneric(context, v0.ToBoolean(context.CultureInfo), ref exceptions);
                    case TypeCode.Byte:
                        return v0 is byte ? fail : TryFromGeneric(context, v0.ToByte(context.CultureInfo), ref exceptions);
                    case TypeCode.Char:
                        return v0 is char ? fail : TryFromGeneric(context, v0.ToChar(context.CultureInfo), ref exceptions);
                    case TypeCode.DateTime:
                        return v0 is DateTime ? fail : TryFromGeneric(context, v0.ToDateTime(context.CultureInfo), ref exceptions);
                    case TypeCode.Decimal:
                        return v0 is decimal ? fail : TryFromGeneric(context, v0.ToDecimal(context.CultureInfo), ref exceptions);
                    case TypeCode.Double:
                        return v0 is double ? fail : TryFromGeneric(context, v0.ToDouble(context.CultureInfo), ref exceptions);
                    case TypeCode.Int16:
                        return v0 is short ? fail : TryFromGeneric(context, v0.ToInt16(context.CultureInfo), ref exceptions);
                    case TypeCode.Int32:
                        return v0 is int ? fail : TryFromGeneric(context, v0.ToInt32(context.CultureInfo), ref exceptions);
                    case TypeCode.Int64:
                        return v0 is long ? fail : TryFromGeneric(context, v0.ToInt64(context.CultureInfo), ref exceptions);
                    case TypeCode.SByte:
                        return v0 is sbyte ? fail : TryFromGeneric(context, v0.ToSByte(context.CultureInfo), ref exceptions);
                    case TypeCode.Single:
                        return v0 is float ? fail : TryFromGeneric(context, v0.ToSingle(context.CultureInfo), ref exceptions);
                    case TypeCode.UInt16:
                        return v0 is ushort ? fail : TryFromGeneric(context, v0.ToUInt16(context.CultureInfo), ref exceptions);
                    case TypeCode.UInt32:
                        return v0 is uint ? fail : TryFromGeneric(context, v0.ToUInt32(context.CultureInfo), ref exceptions);
                    case TypeCode.UInt64:
                        return v0 is ulong ? fail : TryFromGeneric(context, v0.ToUInt64(context.CultureInfo), ref exceptions);
                    case TypeCode.DBNull:
                        return InvokeIForm(context, DBNull.Value, exceptions); ;
                    case TypeCode.String:
                        var s = v0.ToString(context.CultureInfo);
                        if (s == input as string)
                        {
                            return fail;
                        }
                        return InvokeIForm(context, s, exceptions);
                    default:
                        return fail;
                }
            }
            return fail;
        }


        private ConvertResult<T> TryFromGeneric<TInput>(ConvertContext context, TInput input, ref ExceptionCollection exceptions)
        {
            try
            {
                if (this is IFrom<TInput, T> from)
                {
                    var result = from.From(context, input);
                    if (result.Success)
                    {
                        return result;
                    }
                    exceptions += result.Exception;
                }
            }
            catch (Exception e)
            {
                exceptions += this.Error(e, context);
            }
            return new ConvertResult<T>(false, default, null);
        }

        /// <summary>
        /// 优先级 默认0
        /// </summary>
        public virtual uint Priority { get; } = 0;
        public IServiceProvider ServiceProvider { get; }

        protected ConvertResult<T> Ok(T value) => new ConvertResult<T>(value);
    }
}
