using blqw.Kanai.Extensions;
using blqw.Kanai.Interface;
using blqw.Kanai.Interface.From;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace blqw.Kanai.Convertors
{
    class ObjectConvertorFactory : IConvertorFactory
    {
        public IServiceProvider ServiceProvider { get; }

        public ObjectConvertorFactory(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

        public bool CanBuild<T>()
        {
            if (typeof(T).IsMetaType() || typeof(T).IsValueType || typeof(T).IsEnum || typeof(T).IsArray)
            {
                return false;
            }
            if (typeof(T).Namespace.StartsWith("System."))
            {
                return false;
            }
            if (typeof(T).Namespace.StartsWith("Microsoft."))
            {
                return false;
            }

            if (typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Length == 0)
            {
                return false;
            }

            try
            {
                return ActivatorUtilities.CreateInstance<T>(ServiceProvider) != null;
            }
            catch
            {
                return false;
            }
        }

        public IConvertor<T> Build<T>() => CanBuild<T>() ? new ObjectConvertor<T>(ServiceProvider) : null;

        class ObjectConvertor<T> : BaseConvertor<T>
                                 , IFrom<object, T>
        {
            public ObjectConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
            {
            }


            public ConvertResult<T> From(ConvertContext context, object input)
            {
                var enumerator = new KeyValueEnumerator<string, object>(context, input);
                if (!enumerator.HasStringKey)
                {
                    return this.Fail(context, input);
                }
                var obj = ActivatorUtilities.CreateInstance<T>(context.ServiceProvider);
                var properties = PropertyHelper.GetByType(typeof(T));
                var c1 = 0;
                var c2 = 0;
                while (enumerator.MoveNext())
                {
                    c1++;
                    var key = enumerator.GetKey();
                    if (!key.Success)
                    {
                        var message = string.Format(context.ResourceStrings.PROPERTY_CAST_FAIL, typeof(T).GetFriendlyName(), enumerator.OriginalKey);
                        return context.Fail(message, key.Exception);
                    }

                    var prop = properties.FirstOrDefault(x => string.Equals(x.Name, key.OutputValue, StringComparison.OrdinalIgnoreCase));
                    if (prop == null)
                    {
                        continue;
                    }
                    var value = enumerator.OriginalValue.Convert(prop.PropertyType, context);
                    if (!value.Success)
                    {
                        var message = string.Format(context.ResourceStrings.PROPERTY_SET_FAIL, typeof(T).GetFriendlyName(), key.OutputValue, enumerator.OriginalValue);
                        return context.Fail(message, value.Exception);
                    }
                    c2++;
                    prop.SetValue(context.Settings, obj, value.OutputValue);
                }
                if (c1 > 0 && c2 == 0)
                {
                    return this.Fail(context, input);
                }
                return obj;

            }
        }
    }
}
