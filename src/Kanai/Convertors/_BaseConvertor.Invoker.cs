using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace blqw.Kanai.Convertors
{
    public abstract partial class BaseConvertor<T> : IConvertor<T>
    {

        /// <summary>
        /// 基础调用器接口
        /// </summary>
        protected interface IFromInvoker
        {
            /// <summary>
            /// 调用转换方法
            /// </summary>
            /// <param name="ifrom">转换器</param>
            /// <param name="context">转换上下文</param>
            /// <param name="input">输入值</param>
            ConvertResult<T> From(object ifrom, ConvertContext context, object input);
            /// <summary>
            /// 输入类型
            /// </summary>
            Type InputType { get; }
        }

        /// <summary>
        /// 转换器泛型实现
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        private class FromInvoker<TInput> : IFromInvoker
        {
            /// <summary>
            /// 调用转换方法
            /// </summary>
            /// <param name="conv">转换器</param>
            /// <param name="context">转换上下文</param>
            /// <param name="input">输入值</param>
            ConvertResult<T> IFromInvoker.From(object ifrom, ConvertContext context, object input)
                => ((IFrom<TInput, T>)ifrom).From(context, (TInput)input);

            /// <summary>
            /// 输入类型
            /// </summary>
            public Type InputType => typeof(TInput);
        }

        /// <summary>
        /// 调用器字典
        /// </summary>
        private readonly Dictionary<Type, IFromInvoker> _invokers = new Dictionary<Type, IFromInvoker>();


        private void InitInvokers()
        {
            var _interfaceInvokers = new List<IFromInvoker>();
            foreach (var intf in GetType().GetInterfaces())
            {
                if (intf.IsConstructedGenericType && intf.GetGenericTypeDefinition() == typeof(IFrom<,>))
                {
                    var args = intf.GetGenericArguments();
                    if (args[1] == typeof(T))
                    {
                        args[1] = args[0];
                        args[0] = typeof(T);
                        var invoker = (IFromInvoker)Activator.CreateInstance(typeof(FromInvoker<>).MakeGenericType(args));
                        _invokers.Add(invoker.InputType, invoker);
                        if (invoker.InputType.IsInterface)
                        {
                            _interfaceInvokers.Add(invoker);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 获取指定输入类型的调用器
        /// </summary>
        /// <param name="inputType">指定输入类型</param>
        /// <returns></returns>
        protected IEnumerable<IFromInvoker> GetInvokers(Type inputType)
        {
            var invokers = _invokers;
            if (invokers.TryGetValue(inputType, out var invoker0))
            {
                yield return invoker0;
                yield break;
            }

            var interfaceTypes = inputType.GetInterfaces().ToList();

            if (!inputType.IsInterface)
            {
                foreach (var baseType in inputType.EnumerateBaseTypes())
                {
                    if (invokers.TryGetValue(baseType, out var invoker))
                    {
                        yield return invoker;
                        foreach (var i in baseType.GetInterfaces())
                        {
                            interfaceTypes.Remove(i);
                        }
                        break;
                    }
                }
            }

            interfaceTypes.Sort((a, b) => a.IsAssignableFrom(b) ? 1 : b.IsAssignableFrom(a) ? -1 : 0);

            for (var i = 0; i < interfaceTypes.Count; i++)
            {
                var interfaceType = interfaceTypes[i];
                if (invokers.TryGetValue(interfaceType, out var invoker))
                {
                    yield return invoker;
                    foreach (var j in interfaceType.GetInterfaces())
                    {
                        interfaceTypes.Remove(j);
                    }
                }
            }

        }

        protected ConvertResult<T> InvokeIForm(IFromInvoker invoker, ConvertContext context, object input)
        {
            try
            {
                return invoker.From(this, context, input);
            }
            catch (Exception e)
            {
                return e;
            }
        }

        /// <summary>
        /// 调用转换方法
        /// </summary>
        /// <typeparam name="TInput">输入类型</typeparam>
        /// <param name="conv">转换器</param>
        /// <param name="context">转换上下文</param>
        /// <param name="input">输入值</param>
        /// <returns></returns>
        protected ConvertResult<T> InvokeIForm<TInput>(ConvertContext context, TInput input, ExceptionCollection exceptions)
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
                exceptions += e;
            }
            return (Exception)exceptions?.ToConvertException(TypeFriendlyName, input, context.CultureInfo)
                ?? ConvertException.InvalidCast(TypeFriendlyName, null, context.CultureInfo);
        }

    }
}
