using System;
using System.Collections;
using System.Runtime.Serialization;
//using System.Runtime.Remoting;

namespace blqw.Kanai.Dynamic
{
    internal class DynamicEnumerator : DynamicEntity, IObjectReference, IEnumerator//, IObjectHandle, ICustomTypeProvider
    {
        private IEnumerator _enumerator;
        private ConvertSettings _settings;

        public DynamicEnumerator(IEnumerator enumerator)
            : base(enumerator) => _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
        public DynamicEnumerator(IEnumerator enumerator, ConvertSettings settings) : this(enumerator) => _settings = settings;

        public object Current => DynamicFactory.Create(_enumerator, _settings);

        public override Type GetCustomType() => _enumerator?.GetType();

        public override object GetRealObject(StreamingContext context) => _enumerator;

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();

        public override object Unwrap() => _enumerator;
    }
}
