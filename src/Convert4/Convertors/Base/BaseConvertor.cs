using blqw.ConvertServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace blqw.Convertors
{
    /// <summary>
    /// 转换器基类
    /// </summary>
    /// <typeparam name="T">输出类型</typeparam>
    public abstract class BaseConvertor<T> : IConvertor<T>
    {
        /// <summary>
        /// 基础调用器接口
        /// </summary>
        protected interface IInvoker
        {
            /// <summary>
            /// 调用转换方法
            /// </summary>
            /// <param name="conv">转换器</param>
            /// <param name="context">转换上下文</param>
            /// <param name="input">输入值</param>
            ConvertResult<T> ChangeTypeImpl(IConvertor<T> conv, ConvertContext context, object input);
            /// <summary>
            /// 输入类型
            /// </summary>
            Type InputType { get; }
        }

        /// <summary>
        /// 转换器泛型实现
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        private class Invoker<TInput> : IInvoker
        {
            public Invoker()
            {

            }
            /// <summary>
            /// 调用转换方法
            /// </summary>
            /// <param name="conv">转换器</param>
            /// <param name="context">转换上下文</param>
            /// <param name="input">输入值</param>
            ConvertResult<T> IInvoker.ChangeTypeImpl(IConvertor<T> conv, ConvertContext context, object input)
                => InvokeIForm(conv, context, (TInput)input);

            /// <summary>
            /// 输入类型
            /// </summary>
            public Type InputType => typeof(TInput);
        }
        /// <summary>
        /// 调用器字典
        /// </summary>
        private Dictionary<Type, IInvoker> _invokers;
        /// <summary>
        /// 接口类型调用器集合
        /// </summary>
        private readonly List<IInvoker> _interfaceInvokers;

        /// <summary>
        /// 构造函数, 根据实际类型的 <seealso cref="IFrom{TOutput, TInput}"/> 接口情况, 按照 TInput 类型缓存调用器
        /// </summary>
        public BaseConvertor()
        {
            _invokers = new Dictionary<Type, IInvoker>();
            _interfaceInvokers = new List<IInvoker>();
            foreach (var intf in GetType().GetInterfaces())
            {
                if (intf.IsConstructedGenericType && intf.GetGenericTypeDefinition() == typeof(IFrom<,>))
                {
                    var args = intf.GetGenericArguments();
                    if (args[0] == typeof(T))
                    {
                        var invoker = (IInvoker)Activator.CreateInstance(typeof(Invoker<>).MakeGenericType(args));
                        _invokers.Add(invoker.InputType, invoker);
                        if (invoker.InputType.IsInterface)
                        {
                            _interfaceInvokers.Add(invoker);
                        }
                    }
                }
            }
            TypeName = OutputType.FullName;
            TypeFriendlyName = OutputType.GetFriendlyName();
        }

        /// <summary>
        /// 获取指定输入类型的调用器
        /// </summary>
        /// <param name="inputType">指定输入类型</param>
        /// <returns></returns>
        protected IInvoker GetInvoker(Type inputType)
        {
            var invokers = _invokers;
            if (invokers.TryGetValue(inputType, out var invoker0))
            {
                return invoker0;
            }
            if (!inputType.IsInterface)
            {
                foreach (var baseType in inputType.EnumerateBaseTypes())
                {
                    if (invokers.TryGetValue(baseType, out var invoker))
                    {
                        return invoker;
                    }
                }
            }
            var intfs = inputType.GetInterfaces();
            foreach (var invoker in _interfaceInvokers)
            {
                foreach (var intf in intfs)
                {
                    if (invoker.InputType.IsAssignableFrom(intf))
                    {
                        _invokers = new Dictionary<Type, IInvoker>(invokers)
                        {
                            [intf] = invoker
                        };
                        return invoker;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 创造一个返回值实例
        /// </summary>
        protected virtual T CreateOutputInstance(Type type) =>
            (T)Activator.CreateInstance(type);

        /// <summary>
        /// 输出类型
        /// </summary>
        public virtual Type OutputType { get; } = typeof(T);

        /// <summary>
        /// <seealso cref="OutputType"/>.FullName
        /// </summary>
        public virtual string TypeName { get; }

        /// <summary>
        /// <seealso cref="OutputType"/>的友好名称
        /// </summary>
        public virtual string TypeFriendlyName { get; }

        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="context">转换上下文</param>
        /// <param name="input">输入值</param>
        /// <returns></returns>
        ConvertResult IConvertor.ChangeType(ConvertContext context, object input) =>
            ChangeTypeImpl(context, input);

        ConvertResult<T> ChangeTypeImpl(ConvertContext context, object input)
        {
            if (input is T t)
            {
                return new ConvertResult<T>(t);
            }
            //清空异常
            context.Exception = null;
            //空值转换
            if (input == null)
            {
                if (this is IFromNull<T> conv)
                {
                    var result = conv.FromNull(context);
                    return context.Exception ?? new ConvertResult<T>(result);
                }
                return context.InvalidCastException($"`null`{"无法转换为"} {TypeFriendlyName:!}");
            }

            //获取指定输入类型的转换方法调用器
            var invoker = GetInvoker(input.GetType());
            if (invoker != null)
            {
                var result = invoker.ChangeTypeImpl(this, context, input);
                if (result.Success)
                {
                    return result;
                }
                //这里就算异常了,下面还可以尝试其他方案
            }

            //转为各种基本类型进行转换,这次如果失败了就不再继续了
            if (input is IConvertible v0)
            {
                switch (v0.GetTypeCode())
                {
                    case TypeCode.DBNull:
                        if (this is IFromNull<T> conv)
                        {
                            var result = conv.FromDBNull(context);
                            return context.Exception ?? new ConvertResult<T>(result);
                        }
                        return context.Exception = new InvalidCastException(SR.GetString($"`DBNull`{"无法转换为"} {TypeFriendlyName:!}"));
                    case TypeCode.Boolean:
                        return InvokeIForm(this, context, v0.ToBoolean(context.GetCultureInfo()));
                    case TypeCode.Byte:
                        return InvokeIForm(this, context, v0.ToByte(context.GetCultureInfo()));
                    case TypeCode.Char:
                        return InvokeIForm(this, context, v0.ToChar(context.GetCultureInfo()));
                    case TypeCode.DateTime:
                        return InvokeIForm(this, context, v0.ToDateTime(context.GetCultureInfo()));
                    case TypeCode.Decimal:
                        return InvokeIForm(this, context, v0.ToDecimal(context.GetCultureInfo()));
                    case TypeCode.Double:
                        return InvokeIForm(this, context, v0.ToDouble(context.GetCultureInfo()));
                    case TypeCode.Int16:
                        return InvokeIForm(this, context, v0.ToInt16(context.GetCultureInfo()));
                    case TypeCode.Int32:
                        return InvokeIForm(this, context, v0.ToInt32(context.GetCultureInfo()));
                    case TypeCode.Int64:
                        return InvokeIForm(this, context, v0.ToInt64(context.GetCultureInfo()));
                    case TypeCode.SByte:
                        return InvokeIForm(this, context, v0.ToSByte(context.GetCultureInfo()));
                    case TypeCode.Single:
                        return InvokeIForm(this, context, v0.ToSingle(context.GetCultureInfo()));
                    case TypeCode.String:
                        return InvokeIForm(this, context, v0.ToString(context.GetCultureInfo()));
                    case TypeCode.UInt16:
                        return InvokeIForm(this, context, v0.ToUInt16(context.GetCultureInfo()));
                    case TypeCode.UInt32:
                        return InvokeIForm(this, context, v0.ToUInt32(context.GetCultureInfo()));
                    case TypeCode.UInt64:
                        return InvokeIForm(this, context, v0.ToUInt64(context.GetCultureInfo()));
                    default:
                        break;
                }
            }

            //几种可以与string等价的类型,可以尝试转为string后再次转换
            if (input is StringBuilder
                || input is Uri
                || input is IPAddress
                || input is IFormattable)
            {
                var result = context.ChangeType<string>(input);
                if (result.Success)
                {
                    return InvokeIForm(this, context, result.OutputValue);
                }
                //这里失败了 继续尝试object转换方案
            }

            return InvokeIForm(this, context, input);
        }

        /// <summary>
        /// 调用转换方法
        /// </summary>
        /// <typeparam name="TInput">输入类型</typeparam>
        /// <param name="conv">转换器</param>
        /// <param name="context">转换上下文</param>
        /// <param name="input">输入值</param>
        /// <returns></returns>
        static ConvertResult<T> InvokeIForm<TInput>(IConvertor<T> conv, ConvertContext context, TInput input)
        {
            try
            {

                if (conv is IFrom<T, TInput> from)
                {
                    var result = from.From(context, input);
                    if (context.Exception != null)
                    {
                        return context.Exception;
                    }
                    return new ConvertResult<T>(result);
                }
                if (input is IObjectReference refObj)
                {
                    var newObj = refObj.GetRealObject(new StreamingContext(StreamingContextStates.Clone, context));
                    if (!ReferenceEquals(newObj, refObj))
                    {
                        return conv.ChangeType(context, newObj);
                    }
                }
            }
            catch (Exception e)
            {
                return context.Exception = e;
            }

            return context.InvalidCastException(input, (conv as BaseConvertor<T>).TypeFriendlyName ?? conv.OutputType.GetFriendlyName());
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
            return conv as IConvertor<T1> ?? new ProxyConvertor<T1>(conv);
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
    }
}