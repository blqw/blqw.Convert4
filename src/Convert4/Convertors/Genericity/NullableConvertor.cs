using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class NullableConvertor : BaseConvertor<int?>
    {
        public override Type OutputType => typeof(Nullable<>);


        public override IConvertor GetConvertor(Type outputType)
        {
            if (outputType == null)
            {
                throw new ArgumentNullException(nameof(outputType));
            }

            var underlyingType = Nullable.GetUnderlyingType(outputType);
            if (underlyingType == null)
            {
                throw new ArgumentOutOfRangeException(nameof(outputType));
            }
            var args = new Type[] { underlyingType };
            return (IConvertor)Activator.CreateInstance(typeof(InnerConvertor<>).MakeGenericType(args));
        }


        class InnerConvertor<TValue> : BaseConvertor<TValue?>, IConvertor
            where TValue : struct
        {
            public override ConvertResult<TValue?> ChangeType(ConvertContext context, object input)
            {
                if (input == null || input is DBNull || (input is string s && string.IsNullOrWhiteSpace(s)))
                {
                    return new ConvertResult<TValue?>(null);
                }
                var result = context.ChangeType<TValue>(input);
                return result.Cast<TValue?>();
            }

            ConvertResult IConvertor.ChangeType(ConvertContext context, object input)
            {
                if (input == null || input is DBNull || (input is string s && string.IsNullOrWhiteSpace(s)))
                {
                    return new ConvertResult(null);
                }
                return _convertor.ChangeType(context, input);
            }

            private IConvertor<TValue> _convertor;
        }
    }
}
