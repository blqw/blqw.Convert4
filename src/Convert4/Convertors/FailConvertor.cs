using System.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    public sealed class FailConvertor<T> : IConvertor<T>
    {
        private ConvertResult<T> _result;
        private ConvertResult _result2;

        private FailConvertor(Exception exception) => _result2 = _result = exception;



        public ConvertResult<T> ChangeType(ConvertContext context, object input) => _result;

        public IConvertor<T1> GetConvertor<T1>() => new FailConvertor<T1>(_result.Exception);

        public Type OutputType => typeof(T);

        public uint Priority => 0;

        ConvertResult IConvertor.ChangeType(ConvertContext context, object input) => _result2;

        public IConvertor GetConvertor(Type outputType) =>
            (IConvertor)typeof(IConvertor<T>).GetMethod("GetConvertor", BindingFlags.DeclaredOnly)
                                             .MakeGenericMethod(new[] { outputType })
                                             .Invoke(this, Array.Empty<object>());

        public static implicit operator FailConvertor<T>(Exception ex) =>
            ex == null ? null : new FailConvertor<T>(ex);





    }
}
