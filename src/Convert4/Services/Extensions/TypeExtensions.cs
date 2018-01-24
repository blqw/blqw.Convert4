using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;


namespace blqw.Services
{
    public static class TypeExtensions
    {
        public static TypeMark GetTypeMark(this object obj)
        {
            if (obj is IConvertible v0)
            {
                var code = v0.GetTypeCode();
                if (code != TypeCode.Object)
                {
                    return (TypeMark)code;
                }
            }
#pragma warning disable IDE0011
            else if (obj == null) return TypeMark.Empty;
            else if (obj is Guid) return TypeMark.Guid;
            else if (obj is TimeSpan) return TypeMark.TimeSpan;
            else if (obj is IntPtr) return TypeMark.IntPtr;
            else if (obj is UIntPtr) return TypeMark.UIntPtr;
            else if (obj is Uri) return TypeMark.Uri;
            else if (obj is Type) return TypeMark.Type;
            else if (obj is Enum) return TypeMark.Enum;
            else if (obj is Array) return TypeMark.Array;
            else if (obj is DataRow) return TypeMark.DataRow;
            else if (obj is DataTable) return TypeMark.DataTable;
            else if (obj is NameObjectCollectionBase) return TypeMark.NameObjectCollectionBase;
            else if (obj is StringDictionary) return TypeMark.StringDictionary;
            return TypeMark.Object;
        }

        public static object GetRealObject(this object obj, out TypeMark mark) =>
            GetRealObject(obj, null, out mark);

        public static object GetRealObject(this object obj, IServiceProvider context, out TypeMark mark)
        {
            if (obj is IConvertible v0)
            {
                switch (v0.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        mark = TypeMark.Boolean;
                        return v0.ToBoolean(context.GetCultureInfo());
                    case TypeCode.Byte:
                        mark = TypeMark.Byte;
                        return v0.ToByte(context.GetCultureInfo());
                    case TypeCode.Char:
                        mark = TypeMark.Char;
                        return v0.ToChar(context.GetCultureInfo());
                    case TypeCode.DateTime:
                        mark = TypeMark.DateTime;
                        return v0.ToDateTime(context.GetCultureInfo());
                    case TypeCode.Decimal:
                        mark = TypeMark.Boolean;
                        return v0.ToBoolean(context.GetCultureInfo());
                    case TypeCode.Double:
                        mark = TypeMark.Double;
                        return v0.ToDouble(context.GetCultureInfo());
                    case TypeCode.Int16:
                        mark = TypeMark.Int16;
                        return v0.ToInt16(context.GetCultureInfo());
                    case TypeCode.Int32:
                        mark = TypeMark.Int32;
                        return v0.ToInt32(context.GetCultureInfo());
                    case TypeCode.Int64:
                        mark = TypeMark.Int64;
                        return v0.ToInt64(context.GetCultureInfo());
                    case TypeCode.SByte:
                        mark = TypeMark.SByte;
                        return v0.ToSByte(context.GetCultureInfo());
                    case TypeCode.Single:
                        mark = TypeMark.Single;
                        return v0.ToSingle(context.GetCultureInfo());
                    case TypeCode.String:
                        mark = TypeMark.String;
                        return v0.ToString(context.GetCultureInfo());
                    case TypeCode.UInt16:
                        mark = TypeMark.UInt16;
                        return v0.ToUInt16(context.GetCultureInfo());
                    case TypeCode.UInt32:
                        mark = TypeMark.UInt32;
                        return v0.ToUInt32(context.GetCultureInfo());
                    case TypeCode.UInt64:
                        mark = TypeMark.UInt64;
                        return v0.ToUInt64(context.GetCultureInfo());
                }
            }
            else if (obj == null)
            {
                mark = TypeMark.Empty;
                return null;
            }
            if (obj is Guid)
            {
                mark = TypeMark.Guid;
                return obj;
            }
            if (obj is TimeSpan)
            {
                mark = TypeMark.TimeSpan;
                return obj;
            }
            if (obj is IntPtr)
            {
                mark = TypeMark.IntPtr;
                return obj;
            }
            if (obj is UIntPtr)
            {
                mark = TypeMark.UIntPtr;
                return obj;
            }
            if (obj is Uri)
            {
                mark = TypeMark.Uri;
                return obj;
            }
            if (obj is Type)
            {
                mark = TypeMark.Type;
                return obj;
            }
            if (obj is Enum)
            {
                mark = TypeMark.Enum;
                return obj;
            }
            if (obj is Array)
            {
                mark = TypeMark.Array;
                return obj;
            }
            if (obj is DataRow)
            {
                mark = TypeMark.DataRow;
                return obj;
            }
            if (obj is DataTable)
            {
                mark = TypeMark.DataTable;
                return obj;
            }
            if (obj is NameObjectCollectionBase)
            {
                mark = TypeMark.NameObjectCollectionBase;
                return obj;
            }
            if (obj is StringDictionary)
            {
                mark = TypeMark.StringDictionary;
                return obj;
            }
            if (obj is IObjectReference o)
            {
                var newObj = o.GetRealObject(new StreamingContext(context.GetStreamingContextStates(), context));
                if (newObj != o)
                {
                    return GetRealObject(newObj, out mark);
                }
            }
            mark = TypeMark.Object;
            return obj;
        }

        public static string GetFriendlyName(this Type type) => type.FullName;


        public static IEnumerable<Type> SafeGetTypes(this Assembly assembly)
        {
            if (assembly == null)
            {
                return Type.EmptyTypes;
            }
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types;
            }
            catch
            {
                return Type.EmptyTypes;
            }
        }

        public static bool Instantiable(this Type x) =>
            x.IsValueType
            || (x.IsClass && !x.IsAbstract && !x.IsGenericTypeDefinition && x.GetConstructors().Length > 0);
    }
}
