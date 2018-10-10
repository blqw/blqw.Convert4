using blqw.Convertors;
using blqw.ConvertServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace blqw
{
    /// <summary>
    /// 转换器选择器
    /// </summary>
    public class ConvertorSelector : IConvertorSelector
    {
        /// <summary>
        /// 转换器字典
        /// </summary>
        private readonly ConcurrentDictionary<Type, IConvertor> _convertors;
        /// <summary>
        /// <seealso cref="IConvertor{object}"/> 转换器
        /// </summary>
        private readonly IConvertor _objectConvertor;
        /// <summary>
        /// 知否允许衍生新的转换器
        /// </summary>
        private readonly bool _spawnable;

        /// <summary>
        /// 初始化转换器选择器
        /// </summary>
        /// <param name="convertors">基础转换器</param>
        /// <param name="spawnable">是否允许衍生新的转换器</param>
        public ConvertorSelector(IEnumerable<IConvertor> convertors = null, IComparer<Type> typeComparer = null, bool spawnable = true)
        {
            TypeComparer = typeComparer ?? ConvertServices.TypeComparer.Instance;
            _convertors = new ConcurrentDictionary<Type, IConvertor>();
            foreach (var convertor in convertors)
            {
                //同类型转换器只保留优先级最高的, 优先级相同保留顺序靠后的
                if (_convertors.TryGetValue(convertor.OutputType, out var old) && old.Priority > convertor.Priority)
                {
                    continue;
                }
                _convertors[convertor.OutputType] = convertor;
            }
            _convertors.TryGetValue(typeof(object), out _objectConvertor);
            _spawnable = spawnable;
        }

        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <param name="outputType">指定类型</param>
        /// <param name="context">转换上下文</param>
        /// <returns></returns>
        public IConvertor Get(Type outputType, ConvertContext context)
        {
            if (outputType.IsGenericTypeDefinition || outputType.IsAbstract && outputType.IsSealed)
            {
                return null;
            }

            //优先使用注入的服务
            var selector = context.GetService<IConvertorSelector>();
            if (ReferenceEquals(selector, this))
            {
                return selector.Get(outputType, context);
            }

            if (_convertors.TryGetValue(outputType, out var conv) || _spawnable == false)
            {
                // 获取已存在的转换器
                return conv;
            }

            var ee = Match(outputType);
            while (ee.MoveNext())
            {
                conv = ee.Current;
                if (conv.OutputType != outputType)
                {
                    conv = ee.Current.GetConvertor(outputType);
                    if (conv == null)
                    {
                        continue;
                    }
                }
                return _convertors.GetOrAdd(outputType, conv);
            }
            return null;
        }
        /// <summary>
        /// 用于比较服务之间的优先级
        /// </summary>
        private IComparer<Type> TypeComparer { get; }
        /// <summary>
        /// 获取所有匹配类型的转换器
        /// </summary>
        /// <param name="outputType"> </param>
        /// <returns> </returns>
        private IEnumerator<IConvertor> Match(Type outputType)
        {
            var conv = MatchGeneric(outputType);
            if (conv != null)
            {
                yield return conv;
            }

            //匹配父类和接口
            var baseTypes = outputType.EnumerateBaseTypes().Union(outputType.GetInterfaces());
            if (TypeComparer != null)
            {
                //排序
                baseTypes = baseTypes.OrderByDescending(it => it, TypeComparer);
            }
            var convs = _convertors;
            foreach (var type in baseTypes)
            {
                if (convs.TryGetValue(type, out conv))
                {
                    yield return conv;
                }
                else
                {
                    conv = MatchGeneric(type);
                    if (conv != null)
                    {
                        yield return conv;
                    }
                }
            }

            foreach (var conv0 in convs.Values)
            {
                if (outputType.IsAssignableFrom(conv0.OutputType))
                {
                    yield return conv0;
                }
            }

            yield return _objectConvertor;
        }
        /// <summary>
        /// 获取与 <paramref name="genericType" /> 的泛型定义类型匹配的转换器,如果
        /// <paramref name="genericType" /> 不是泛型,返回 null
        /// </summary>
        /// <param name="genericType"> 用于匹配的 <see cref="Type" /> </param>
        /// <returns> </returns>
        private IConvertor MatchGeneric(Type genericType)
        {
            if (genericType.IsGenericType && (genericType.IsGenericTypeDefinition == false))
            {
                if (_convertors.TryGetValue(genericType.GetGenericTypeDefinition(), out var conv))
                {
                    return conv;
                }
            }
            return null;
        }
    }
}
