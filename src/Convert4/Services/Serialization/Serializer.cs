using System;

namespace blqw.ConvertServices
{
    class Serializer : ISerializationService
    {
        private readonly Func<object, string> _toString;
        private readonly Func<string, Type, object> _toObject;

        public Serializer(string contract, Func<object, string> toString, Func<string, Type, object> toObject)
        {
            Contract = contract;
            _toString = toString ?? throw new ArgumentNullException(nameof(toString));
            _toObject = toObject ?? throw new ArgumentNullException(nameof(toObject));
        }

        public string Contract { get; }

        public string ToString(object value) => _toString(value);
        public T ToObject<T>(string value) => (T)_toObject(value, typeof(T));
    }
}
