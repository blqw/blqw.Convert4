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
            if (outputType == null)
            {
                throw new ArgumentNullException(nameof(outputType));
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
                if (builder.TryCreateInstance() == false)
                {
                    return default;
                }

                var mapper = new Mapper(input);

                if (mapper.Error != null)
                {
                    context.InvalidCastException(mapper.Error);
                    return default;
                }

                while (mapper.MoveNext())
                {
                    if (builder.Add(mapper.Key, mapper.Value) == false)
                    {
                        return default;
                    }
                }
                return (T)builder.Instance;
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
                        _context.InvalidOperationException($"{_type.GetFriendlyName():!} {"构造"}{"失败"},{"原因:"}{"成员名称"}{"转换失败"}");
                        return false;
                    }
                    var p = _propertyHandlers.FirstOrDefault(x => x.Name.Equals(propName.OutputValue, StringComparison.OrdinalIgnoreCase));
                    if (p != null)
                    {
                        if(p.SetValue(_context, Instance, value) == false)
                        {
                            _context.InvalidOperationException($"{_type.GetFriendlyName():!} {"构造"}{"失败"},{"原因:"}{"成员"}:{p.Name}{"转换失败"}");
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
                        _context.InvalidOperationException($"{"创建"} {_type.GetFriendlyName()} {"失败"},{"原因"}:{ex.Message}");
                        return false;
                    }
                }
            }
        }
    }
}
