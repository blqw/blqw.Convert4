using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace blqw.Convertors
{
    class NameValueCollectionConvertor : BaseConvertor<NameValueCollection>,
                                         IFrom<object, NameValueCollection>
    {
        public NameValueCollection From(ConvertContext context, object input)
        {
            if (input is null || input is DBNull)
            {
                return null;
            }

            var builder = new NVCollectiontBuilder(context, OutputType);
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
;
            return builder.Instance;
        }


        /// <summary>
        /// <seealso cref="NameValueCollection" /> 转换器
        /// </summary>
        private struct NVCollectiontBuilder
        {
            private readonly ConvertContext _context;
            private readonly Type _type;
            /// <summary>
            /// 键转换器
            /// </summary>
            private readonly IConvertor<string> _stringConvertor;

            public NVCollectiontBuilder(ConvertContext context, Type type)
            {
                _context = context;
                _type = type;
                _stringConvertor = context.GetConvertor<string>();
                Instance = null;
            }

            /// <summary>
            /// 被构造的实例
            /// </summary>
            public NameValueCollection Instance { get; private set; }

            /// <summary>
            /// 设置对象值
            /// </summary>
            /// <param name="obj"> 待设置的值 </param>
            /// <returns> </returns>
            public bool Set(DictionaryEntry obj) => Add(obj.Key, obj.Value);

            /// <summary>
            /// 尝试构造实例,返回是否成功
            /// </summary>
            /// <returns> </returns>
            public bool TryCreateInstance()
            {
                try
                {
                    Instance = (NameValueCollection)Activator.CreateInstance(_type);
                    return true;
                }
                catch (Exception ex)
                {
                    _context.Error.AddException(ex);
                    _context.InvalidOperationException($"{"创建"} NameValueCollection {"失败"},{"原因"}:{ex.Message}");
                    return false;
                }
            }

            public bool Add(object key, object value)
            {
                var conv = _context.GetConvertor<string>();
                var rkey = conv.ChangeType(_context, key);
                if (rkey.Success == false)
                {
                    _context.InvalidOperationException($"{"向"} NameValueCollection {"添加元素"}{"失败"},{"原因:"}Key{"转换失败"}");
                    return false;
                }
                var rval = conv.ChangeType(_context, value);
                if (rval.Success == false)
                {
                    _context.InvalidOperationException($"{"向"} NameValueCollection {"添加元素"} {rkey.OutputValue:!} {"失败"},{"原因:"}Value{"转换失败"}");
                    return false;
                }
                try
                {
                    Instance.Add(rkey.OutputValue, rval.OutputValue);
                    return true;
                }
                catch (Exception ex)
                {
                    _context.Error.AddException(ex);
                    _context.InvalidOperationException($"{"向"} NameValueCollection {"添加元素"} {rkey.OutputValue:!} {"失败"},{"原因:"}{ex.Message}");
                    return false;
                }
            }
        }
    }
}
