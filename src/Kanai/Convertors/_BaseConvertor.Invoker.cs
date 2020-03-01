using blqw.Kanai.Extensions;
using blqw.Kanai.Froms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blqw.Kanai.Convertors
{
    public abstract partial class BaseConvertor<T> : IConvertor<T>
    {

        /// <summary>
        /// 调用器字典
        /// </summary>
        private readonly Dictionary<Type, InvokeIFormHandler> _invokers = new Dictionary<Type, InvokeIFormHandler>();


        private void InitInvokers()
        {
            var method = ((Func<InvokeIFormHandler>)CreateInvoker<object>).Method.GetGenericMethodDefinition();
            foreach (var intf in GetType().GetInterfaces())
            {
                if (intf.IsConstructedGenericType && intf.GetGenericTypeDefinition() == typeof(IFrom<,>))
                {
                    var args = intf.GetGenericArguments();
                    if (args[1] == typeof(T))
                    {
                        var invoker = (InvokeIFormHandler)method.MakeGenericMethod(args[0]).Invoke(null, null);
                        _invokers.Add(args[0], invoker);
                    }
                }
            }
        }


        /// <summary>
        /// 获取指定输入类型的调用器
        /// </summary>
        /// <param name="inputType">指定输入类型</param>
        /// <returns></returns>
        protected IEnumerable<InvokeIFormHandler> GetInvokers(Type inputType)
        {
            if (_invokers.Count == 0)
            {
                yield break;
            }
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


        protected delegate ConvertResult<T> InvokeIFormHandler(BaseConvertor<T> convertor, ConvertContext context, object input);

        private static InvokeIFormHandler CreateInvoker<TInput>()
            => (convertor, context, input) =>
            {
                try
                {
                    return ((IFrom<TInput, T>)convertor).From(context, (TInput)input);
                }
                catch (Exception e)
                {
                    return convertor.Error(e, context);
                }
            };

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
                exceptions += this.Error(e, context);
            }
            return this.Fail(null, context, exceptions);
        }

    }
}
