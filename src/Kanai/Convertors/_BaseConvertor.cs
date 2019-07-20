﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Collections;
using System.Data;
using System.ComponentModel;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// 转换器基类
    /// </summary>
    /// <typeparam name="T">输出类型</typeparam>
    public abstract partial class BaseConvertor<T> : IConvertor<T>
    {

        /// <summary>
        /// 构造函数, 根据实际类型的 <seealso cref="IFrom{TOutput, TInput}"/> 接口情况, 按照 TInput 类型缓存调用器
        /// </summary>
        public BaseConvertor()
            : this(typeof(T))
        {
        }

        protected BaseConvertor(Type outputType)
        {
            InitInvokers();
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


        private Exception Fail(ConvertContext context, object input, ExceptionCollection exceptions)
            => exceptions == null ? ConvertException.InvalidCast(TypeFriendlyName, null, context.CultureInfo)
                 : (Exception)exceptions?.ToConvertException(TypeFriendlyName, input, context.CultureInfo);

        ConvertResult<T> ChangeTypeImpl(ConvertContext context, object input)
        {
            if (input is T t)
            {
                return new ConvertResult<T>(t);
            }

            ExceptionCollection exceptions = null;

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

                return Fail(context, input, exceptions);
            }


            //获取指定输入类型的转换方法调用器
            var invokers = GetInvokers(input.GetType());
            foreach (var invoker in invokers)
            {
                var result = InvokeIForm(invoker, context, input);
                if (result.Success)
                {
                    return result;
                }
                //如果异常,下面还可以尝试其他方案
                exceptions += result.Exception;
            }

            // 转为各种基本类型进行转换, 这里也可以使用翻译器来做, 但是考虑值类型装箱拆箱引起的性能问题, 决定写死
            if (input is IConvertible v0)
            {
                switch (v0.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return v0 is bool ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToBoolean(context.CultureInfo), exceptions);
                    case TypeCode.Byte:
                        return v0 is byte ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToByte(context.CultureInfo), exceptions);
                    case TypeCode.Char:
                        return v0 is char ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToChar(context.CultureInfo), exceptions);
                    case TypeCode.DateTime:
                        return v0 is DateTime ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToDateTime(context.CultureInfo), exceptions);
                    case TypeCode.Decimal:
                        return v0 is decimal ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToDecimal(context.CultureInfo), exceptions);
                    case TypeCode.Double:
                        return v0 is double ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToDouble(context.CultureInfo), exceptions);
                    case TypeCode.Int16:
                        return v0 is short ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToInt16(context.CultureInfo), exceptions);
                    case TypeCode.Int32:
                        return v0 is int ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToInt32(context.CultureInfo), exceptions);
                    case TypeCode.Int64:
                        return v0 is long ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToInt64(context.CultureInfo), exceptions);
                    case TypeCode.SByte:
                        return v0 is sbyte ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToSByte(context.CultureInfo), exceptions);
                    case TypeCode.Single:
                        return v0 is float ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToSingle(context.CultureInfo), exceptions);
                    case TypeCode.UInt16:
                        return v0 is ushort ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToUInt16(context.CultureInfo), exceptions);
                    case TypeCode.UInt32:
                        return v0 is uint ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToUInt32(context.CultureInfo), exceptions);
                    case TypeCode.UInt64:
                        return v0 is ulong ? Fail(context, input, exceptions) : InvokeIForm(context, v0.ToUInt64(context.CultureInfo), exceptions);
                    case TypeCode.DBNull:
                        return Fail(context, input, exceptions);
                    case TypeCode.String:
                        if (input is string)
                        {
                            return Fail(context, input, exceptions);
                        }
                        var s = v0.ToString(context.CultureInfo);
                        if (s == input as string)
                        {
                            return Fail(context, input, exceptions);
                        }
                        return InvokeIForm(context, s, exceptions);
                    default:
                        break;
                }
            }

            // 使用翻译器转换为其他类型后尝试转型
            foreach (var value in context.Translate(input))
            {
                var invokers0 = GetInvokers(value.GetType());
                foreach (var invoker in invokers0)
                {
                    var result = InvokeIForm(invoker, context, value);
                    if (result.Success)
                    {
                        return result;
                    }
                    //如果异常,下面还可以尝试其他方案
                    exceptions += result.Exception;
                }
            }

            return InvokeIForm(context, input, exceptions);

        }

        /// <summary>
        /// 转换转换方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual ConvertResult<T> ChangeType(ConvertContext context, object input) =>
            ChangeTypeImpl(context, input);

        /// <summary>
        /// 获取子转换器
        /// </summary>
        /// <param name="outputType">子转换器输出类型</param>
        /// <returns></returns>
        public virtual IConvertor GetConvertor(Type outputType) => null;

        /// <summary>
        /// 优先级 默认0
        /// </summary>
        public virtual uint Priority { get; } = 0;


        protected ConvertResult<T> Result(T value) => new ConvertResult<T>(value);
    }
}