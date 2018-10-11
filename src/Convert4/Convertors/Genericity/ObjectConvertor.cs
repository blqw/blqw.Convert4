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
            public ConvertResult<T> From(ConvertContext context, string input)
            {
                var serializer = context.GetSerializer();
                if (serializer == null)
                {
                    return context.InvalidOperationException($"缺少序列化服务");
                }
                return (T)serializer.ToObject(input, typeof(T));
            }

            public ConvertResult<T> From(ConvertContext context, object input)
            {
                var builder = new ObjectBuilder(context);
                var ex = builder.Exception + Mapper.Build(context, OutputType, input, builder.InstanceCreated, builder.Add);
                return ex ?? Result(builder.Instance);
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

                public ObjectBuilder(ConvertContext context)
                {
                    _type = typeof(T);
                    _context = context;
                    _keyConvertor = context.GetConvertor<string>();
                    _propertyHandlers = PropertyHelper.GetByType(_type);
                    try
                    {
                        Instance = (T)_context.CreateInstance(_type);
                        Exception = null;
                    }
                    catch (Exception ex)
                    {
                        Instance = default;
                        Exception = ex as ConvertException ?? new ConvertException(ex);
                    }
                }
                public ConvertException Exception { get; }
                /// <summary>
                /// 被构造的实例
                /// </summary>
                public T Instance { get; }

                public bool InstanceCreated => Exception == null;

                public ConvertException Add(object key, object value)
                {
                    var prop = _keyConvertor.ChangeType(_context, key);
                    if (prop.Success == false)
                    {
                        return prop.Exception + _context.InvalidCastException($"{"属性名"}{"转换失败"}");
                    }
                    var p = _propertyHandlers.FirstOrDefault(x => x.Name.Equals(prop.OutputValue, StringComparison.OrdinalIgnoreCase));
                    if (p != null)
                    {
                        var ex = p.SetValue(_context, Instance, value);
                        if (ex != null)
                        {
                            return ex;
                        }
                    }
                    return null;
                }
            }
        }
    }
}
