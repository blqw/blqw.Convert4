using blqw.Kanai.Interface;
using System;

namespace blqw.Core
{
    public class StringSerializer : IStringSerializer
    {
        private readonly Func<object, string> _toString;
        private readonly Func<string, Type, object> _toObject;

        public StringSerializer(string protocol, Func<object, string> toString, Func<string, Type, object> toObject)
        {
            Protocol = protocol?.ToLowerInvariant();
            _toString = toString ?? throw new ArgumentNullException(nameof(toString));
            _toObject = toObject ?? throw new ArgumentNullException(nameof(toObject));
        }

        public string Protocol { get; }

        public string ToString(object value) => _toString(value);
        public T ToObject<T>(string value) => (T)_toObject(value, typeof(T));
        public object ToObject(string value, Type type) => _toObject(value, type);
    }
}
