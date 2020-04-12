using blqw.Kanai.Extensions;
using blqw.Kanai.Interface;
using blqw.Kanai.Interface.From;
using System;
using System.Collections.Generic;

namespace blqw.Kanai.Convertors
{
    public abstract partial class BaseConvertor<T> : IConvertor<T>
    {


        private Dictionary<Type, InvokeIFormHandler> InitInvokers()
        {
            var invokers = new Dictionary<Type, InvokeIFormHandler>();
            var method = ((Func<InvokeIFormHandler>)CreateInvoker<object>).Method.GetGenericMethodDefinition();
            foreach (var intf in GetType().GetInterfaces())
            {
                if (intf.IsConstructedGenericType && intf.GetGenericTypeDefinition() == typeof(IFrom<,>))
                {
                    var args = intf.GetGenericArguments();
                    if (args[1] == typeof(T))
                    {
                        var invoker = (InvokeIFormHandler)method.MakeGenericMethod(args[0]).Invoke(null, null);
                        invokers.Add(args[0], invoker);
                    }
                }
            }
            return invokers;
        }

        /// <summary>
        /// 精确匹配调度器
        /// </summary>
        protected InvokeIFormHandler GetInvoker(Type inputType)
        {
            if (_invokers.TryGetValue(inputType, out var invoker0))
            {
                return invoker0;
            }
            return null;
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

            foreach (var type in _invokerTypes)
            {
                if (type.IsAssignableFrom(inputType))
                {
                    yield return invokers[type];
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
            return this.Fail(context, null, exceptions);
        }


    }
}
