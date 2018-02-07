using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Collections.Concurrent;

namespace blqw.Services
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取类型标记
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取真实对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static object GetRealObject(this object obj, out TypeMark mark) =>
            GetRealObject(obj, null, out mark);
        /// <summary>
        /// 获取真实对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="context"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 安全的获取程序集中可以被导出的类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 判断类型是否可被实例化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Instantiable(this Type type) =>
            type.IsValueType
            || (type.IsClass && !type.IsAbstract && !type.IsGenericTypeDefinition && type.GetConstructors().Length > 0);

        /// <summary>
        /// 获取当前类型根据指定泛型定义类型约束导出的泛型参数
        /// </summary>
        /// <param name="type"> 测试类型 </param>
        /// <param name="defineType"> 泛型定义类型 </param>
        /// <param name="inherit"> 是否检测被测试类型的父类和接口 </param>
        /// <returns></returns>
        public static Type[] GetGenericArguments(this Type type, Type defineType, bool inherit = true)
        {
            if (defineType.IsAssignableFrom(type)) //2个类本身存在继承关系
            {
                return type.GetGenericArguments();
            }
            if (defineType.IsGenericType == false)
            {
                return null; //否则如果definer不是泛型类,则不存在兼容的可能性
            }
            if (defineType.IsGenericTypeDefinition == false)
            {
                defineType = defineType.GetGenericTypeDefinition(); //获取定义类型的泛型定义
            }
            if (type.IsGenericType)
            {
                if (type.IsGenericTypeDefinition)
                {
                    return null; //tester是泛型定义类型,无法兼容
                }
                //获取2个类的泛型参数
                var arg1 = ((TypeInfo)defineType).GenericTypeParameters;
                var arg2 = type.GetGenericArguments();
                //判断2个类型的泛型参数个数
                if (arg1.Length == arg2.Length)
                {
                    //判断definer 应用 tester泛型参数 后的继承关系
                    if (defineType.MakeGenericType(arg2).IsAssignableFrom(type))
                    {
                        return arg2;
                    }
                }
            }
            if (inherit == false)
            {
                return null;
            }
            //测试tester的父类是否被definer兼容
            var baseType = type.BaseType;
            while ((baseType != typeof(object)) && (baseType != null))
            {
                var result = GetGenericArguments(baseType, defineType, false);
                if (result != null)
                {
                    return result;
                }
                baseType = baseType.BaseType;
            }
            //测试tester的接口是否被definer兼容
            foreach (var @interface in type.GetInterfaces())
            {
                var result = GetGenericArguments(@interface, defineType, false);
                if (result != null)
                {
                    return result;
                }
            }
            return null;

        }

        /// <summary>
        /// 枚举所有父类
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public static IEnumerable<Type> EnumerateBaseTypes(this Type type)
        {
            var baseType = type?.BaseType ?? typeof(object);
            while (baseType != typeof(object))
            {
                yield return baseType;
                baseType = baseType.BaseType ?? typeof(object);
            }
        }


        /// <summary>
        /// 类型名称缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, string> _typeNames = new ConcurrentDictionary<Type, string>();
        private static readonly Type System_IValueTupleInternal_Type = Type.GetType("System.IValueTupleInternal", false, false);

        /// <summary>
        /// 获取类型名称的友好展现形式
        /// </summary>
        public static string GetFriendlyName(this Type type)
        {
            if (type == null)
            {
                return "`null`";
            }

            return _typeNames.GetOrAdd(type, t =>
            {
                var t2 = Nullable.GetUnderlyingType(t);
                if (t2 != null)
                {
                    return GetFriendlyName(t2) + "?";
                }
                if (t.IsPointer)
                {
                    return GetFriendlyName(t.GetElementType()) + "*";
                }
                if (t.IsByRef)
                {
                    return GetFriendlyName(t.GetElementType()) + "&";
                }
                if (t.IsArray)
                {
                    return GetFriendlyName(t.GetElementType()) + "[]";
                }
                if (!t.IsGenericType)
                {
                    return GetSimpleName(t);
                }
                string[] generic;
                if (t.IsGenericTypeDefinition) //泛型定义
                {
                    var args = t.GetGenericArguments();
                    generic = new string[args.Length];
                }
                else
                {
                    var infos = t.GetGenericArguments();
                    generic = new string[infos.Length];
                    for (var i = 0; i < infos.Length; i++)
                    {
                        generic[i] = GetFriendlyName(infos[i]);
                    }

                    //这个表示元组类型
                    if ((System_IValueTupleInternal_Type?.IsAssignableFrom(t) ?? false))
                    {
                        return "(" + string.Join(", ", generic) + ")";
                    }
                }
                return GetSimpleName(t) + "<" + string.Join(", ", generic) + ">";
            });
        }

        private static string GetSimpleName(this Type t)
        {
            string name;
            if (t.ReflectedType == null)
            {
                switch (t.Namespace)
                {
                    case "System":
                        switch (t.Name)
                        {
                            case "Boolean":
                                return "bool";
                            case "Byte":
                                return "byte";
                            case "Char":
                                return "char";
                            case "Decimal":
                                return "decimal";
                            case "Double":
                                return "double";
                            case "Int16":
                                return "short";
                            case "Int32":
                                return "int";
                            case "Int64":
                                return "long";
                            case "SByte":
                                return "sbyte";
                            case "Single":
                                return "float";
                            case "String":
                                return "string";
                            case "Object":
                                return "object";
                            case "UInt16":
                                return "ushort";
                            case "UInt32":
                                return "uint";
                            case "UInt64":
                                return "ulong";
                            case "Guid":
                                return "Guid";
                            case "Void":
                                return "void";
                            default:
                                name = t.Name;
                                break;
                        }
                        break;
                    case null:
                    case "System.Collections":
                    case "System.Collections.Generic":
                    case "System.Data":
                    case "System.Text":
                    case "System.IO":
                    case "System.Collections.Specialized":
                        name = t.Name;
                        break;
                    default:
                        name = $"{t.Namespace}.{t.Name}";
                        break;
                }
            }
            else
            {
                name = $"{GetSimpleName(t.ReflectedType)}.{t.Name}";
            }
            var index = name.LastIndexOf('`');
            if (index > -1)
            {
                name = name.Remove(index);
            }
            return name;
        }
    }
}
