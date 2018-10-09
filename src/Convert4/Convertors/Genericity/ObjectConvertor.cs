using blqw.ConvertServices;
using blqw.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace blqw.Convertors
{
    class ObjectConvertor : BaseConvertor<object>
    {
        public override IConvertor GetConvertor(Type outputType)
        {
            if (outputType == typeof(object))
            {
                return this;
            }

            var args = new Type[] { outputType };
            return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<>).MakeGenericType(args));
        }

        class InnerConvertor<T> : BaseConvertor<T>
                                , IFrom<string, T>
                                , IFrom<object, T>
        {
            public T From(ConvertContext context, string input)
            {
                var serializer = context.GetSerializer();
                if (serializer == null)
                {
                    context.InvalidOperationException($"缺少序列化服务");
                    return default;
                }
                return (T)serializer.ToObject(input, typeof(T));
            }

            public T From(ConvertContext context, object input)
            {
                var builder = new ObjectBuilder(typeof(T), context);
                if (Mapper.Build(context, OutputType, input, builder.TryCreateInstance, builder.Add))
                {
                    return (T)builder.Instance;
                }
                return default;
            }


            /// <summary>
            /// 构造器
            /// </summary>
            private struct ObjectBuilder
            {
                private readonly Type _type;
                private readonly ConvertContext _context;
                private PropertyHandler[] _propertyHandlers;
                /// <summary>
                /// 键转换器
                /// </summary>
                private readonly IConvertor<string> _keyConvertor;

                public ObjectBuilder(Type type, ConvertContext context)
                {
                    _type = type;
                    _context = context;
                    _keyConvertor = context.GetConvertor<string>();
                    Instance = null;
                    _propertyHandlers = PropertyHelper.GetByType(type);
                }

                /// <summary>
                /// 被构造的实例
                /// </summary>
                public object Instance { get; private set; }

                public bool Add(object key, object value)
                {
                    var propName = _keyConvertor.ChangeType(_context, key);
                    if (propName.Success == false)
                    {
                        _context.InvalidCastException($"{"属性名"}{"转换失败"}");
                        return false;
                    }
                    var p = _propertyHandlers.FirstOrDefault(x => x.Name.Equals(propName.OutputValue, StringComparison.OrdinalIgnoreCase));
                    if (p != null)
                    {
                        if(p.SetValue(_context, Instance, value) == false)
                        {
                            _context.InvalidCastException($"{"属性"} {propName.OutputValue:!} {"转换失败"}");
                            return false;
                        }
                    }
                    return true;
                }

                /// <summary>
                /// 尝试构造实例,返回是否成功
                /// </summary>
                /// <returns> </returns>
                public bool TryCreateInstance()
                {
                    try
                    {
                        Instance = _context.CreateInstance(_type);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _context.Error.AddException(ex);
                        return false;
                    }
                }
            }
        }
    }
}
