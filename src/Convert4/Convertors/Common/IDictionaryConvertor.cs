using blqw.ConvertServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class IDictionaryConvertor : BaseConvertor<IDictionary>,
                               IFrom<object, IDictionary>
    {
        public IDictionary From(ConvertContext context, object input)
        {
            if (input is null || input is DBNull)
            {
                return null;
            }

            var builder = new DictionaryBuilder(OutputType, context);
            if (builder.TryCreateInstance() == false)
            {
                return null;
            }

            var mapper = new Mapper(input);

            if (mapper.Error != null)
            {
                context.InvalidCastException(mapper.Error);
                return null;
            }

            while (mapper.MoveNext())
            {
                if (builder.Add(mapper.Key, mapper.Value) == false)
                {
                    return null;
                }
            }
            return builder.Instance;
        }

        /// <summary>
        /// <seealso cref="IDictionary" /> 构造器
        /// </summary>
        private struct DictionaryBuilder
        {
            private readonly Type _type;
            private readonly ConvertContext _context;

            public DictionaryBuilder(Type type, ConvertContext context)
            {
                _type = type;
                _context = context;
                Instance = null;
            }

            /// <summary>
            /// 被构造的实例
            /// </summary>
            public IDictionary Instance { get; private set; }

            public bool Add(object key, object value)
            {
                try
                {
                    Instance.Add(key, value);
                    return true;
                }
                catch (Exception ex)
                {
                    _context.Exception = new InvalidOperationException($"向字典{_type.GetFriendlyName()}中添加元素 {key} 失败,原因:{ex.Message}", ex);
                    return false;
                }
            }

            /// <summary>
            /// 尝试构造实例,返回是否成功
            /// </summary>
            /// <returns> </returns>
            public bool TryCreateInstance()
            {
                if (_type.IsInterface)
                {
                    Instance = new Hashtable();
                    return true;
                }
                try
                {
                    Instance = (IDictionary)Activator.CreateInstance(_type);
                    return true;
                }
                catch (Exception ex)
                {
                    _context.Exception = ex;
                    return false;
                }
            }

            /// <summary>
            /// 设置对象值
            /// </summary>
            /// <param name="obj"> 待设置的值 </param>
            /// <returns> </returns>
            public bool Set(DictionaryEntry obj) => Add(obj.Key, obj.Value);
        }
    }
}
