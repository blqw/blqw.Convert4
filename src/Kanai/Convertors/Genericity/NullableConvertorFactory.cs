using blqw.Kanai.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace blqw.Kanai.Convertors
{
    class NullableConvertorFactory : IConvertorFactory
    {
        public NullableConvertorFactory(IServiceProvider serviceProvider) =>
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public IServiceProvider ServiceProvider { get; }

        public IConvertor<T> Build<T>()
        {
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            if (underlyingType == null)
            {
                return null;
            }
            return (IConvertor<T>)ActivatorUtilities.GetServiceOrCreateInstance(ServiceProvider, typeof(NullableConvertor<>).MakeGenericType(underlyingType));
        }
        public bool CanBuild<T>() => Nullable.GetUnderlyingType(typeof(T)) != null;

        class NullableConvertor<TValue> : BaseConvertor<TValue?>, IConvertor<TValue?>
            where TValue : struct
        {
            public NullableConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
            {
            }

            public override ConvertResult<TValue?> Convert(ConvertContext context, object input)
            {
                if (input == null || input is DBNull || (input is string s && string.IsNullOrWhiteSpace(s)))
                {
                    return new ConvertResult<TValue?>(null);
                }
                var result = context.Convert<TValue>(input);
                return result.Success ? (ConvertResult<TValue?>)result.OutputValue : result.Exception;
            }

        }

    }
}
