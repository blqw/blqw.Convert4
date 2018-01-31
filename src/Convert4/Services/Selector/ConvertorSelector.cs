using System.Runtime.InteropServices.ComTypes;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using blqw.Services;
using System.Linq;

namespace blqw
{
    public sealed class ConvertorSelector : IConvertorSelector
    {
        private readonly ConcurrentDictionary<Type, IConvertor> _convertors;
        IConvertor _objectConvertor;

        public ConvertorSelector(IEnumerable<IConvertor> convertors)
        {
            TypeComparer = new TypeComparer();
            _convertors = new ConcurrentDictionary<Type, IConvertor>();
            foreach (var convertor in convertors)
            {
                _convertors.AddOrUpdate(convertor.OutputType, convertor, (t, c) => c.Priority < convertor.Priority ? convertor : c);
            }
            _convertors.TryGetValue(typeof(object), out _objectConvertor);
        }

        public IConvertor<T> Get<T>(ConvertContext context) =>
            (IConvertor<T>)Get(typeof(T), context);

        public IConvertor Get(Type outputType, ConvertContext context)
        {
            if (_convertors.TryGetValue(outputType, out var conv))
            {
                return conv;
            }

            var ee = Match(outputType);
            while (ee.MoveNext())
            {
                conv = ee.Current?.GetConvertor(outputType);
                if (conv != null)
                {
                    _convertors.TryAdd(outputType, conv);
                    return conv;
                }
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
                baseTypes = baseTypes.OrderByDescending(it => it, TypeComparer);
            }

            foreach (var interfaceType in baseTypes)
            {
                if (_convertors.TryGetValue(interfaceType, out conv))
                {
                    yield return conv;
                }
                else
                {
                    conv = MatchGeneric(interfaceType);
                    if (conv != null)
                    {
                        yield return conv;
                    }
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
