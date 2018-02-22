using System.Linq;
using blqw.ConvertServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Collections;
using System.Data;
using System.ComponentModel;

namespace blqw.Convertors
{
    /// <summary>
    /// ת��������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    public abstract class BaseConvertor<T> : IConvertor<T>
    {
        /// <summary>
        /// �����������ӿ�
        /// </summary>
        protected interface IInvoker
        {
            /// <summary>
            /// ����ת������
            /// </summary>
            /// <param name="conv">ת����</param>
            /// <param name="context">ת��������</param>
            /// <param name="input">����ֵ</param>
            ConvertResult<T> ChangeTypeImpl(IConvertor<T> conv, ConvertContext context, object input);
            /// <summary>
            /// ��������
            /// </summary>
            Type InputType { get; }
        }

        /// <summary>
        /// ת��������ʵ��
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        private class Invoker<TInput> : IInvoker
        {
            public Invoker()
            {

            }
            /// <summary>
            /// ����ת������
            /// </summary>
            /// <param name="conv">ת����</param>
            /// <param name="context">ת��������</param>
            /// <param name="input">����ֵ</param>
            ConvertResult<T> IInvoker.ChangeTypeImpl(IConvertor<T> conv, ConvertContext context, object input)
                => InvokeIForm(conv, context, (TInput)input);

            /// <summary>
            /// ��������
            /// </summary>
            public Type InputType => typeof(TInput);
        }
        /// <summary>
        /// �������ֵ�
        /// </summary>
        private Dictionary<Type, IInvoker> _invokers;
        /// <summary>
        /// �ӿ����͵���������
        /// </summary>
        private readonly List<IInvoker> _interfaceInvokers;

        /// <summary>
        /// ���캯��, ����ʵ�����͵� <seealso cref="IFrom{TOutput, TInput}"/> �ӿ����, ���� TInput ���ͻ��������
        /// </summary>
        public BaseConvertor()
        {
            _invokers = new Dictionary<Type, IInvoker>();
            _interfaceInvokers = new List<IInvoker>();
            foreach (var intf in GetType().GetInterfaces())
            {
                if (intf.IsConstructedGenericType && intf.GetGenericTypeDefinition() == typeof(IFrom<,>))
                {
                    var args = intf.GetGenericArguments();
                    if (args[1] == typeof(T))
                    {
                        var invoker = (IInvoker)Activator.CreateInstance(typeof(Invoker<>).MakeGenericType(new Type[] { args[1], args[0] }));
                        _invokers.Add(invoker.InputType, invoker);
                        if (invoker.InputType.IsInterface)
                        {
                            _interfaceInvokers.Add(invoker);
                        }
                    }
                }
            }
            TypeName = OutputType.FullName;
            TypeFriendlyName = OutputType.GetFriendlyName();
        }

        /// <summary>
        /// ��ȡָ���������͵ĵ�����
        /// </summary>
        /// <param name="inputType">ָ����������</param>
        /// <returns></returns>
        protected IInvoker GetInvoker(Type inputType)
        {
            var invokers = _invokers;
            if (invokers.TryGetValue(inputType, out var invoker0))
            {
                return invoker0;
            }
            if (!inputType.IsInterface)
            {
                foreach (var baseType in inputType.EnumerateBaseTypes())
                {
                    if (invokers.TryGetValue(baseType, out var invoker))
                    {
                        return invoker;
                    }
                }
            }
            var intfs = inputType.GetInterfaces();
            foreach (var invoker in _interfaceInvokers)
            {
                foreach (var intf in intfs)
                {
                    if (invoker.InputType.IsAssignableFrom(intf))
                    {
                        _invokers = new Dictionary<Type, IInvoker>(invokers)
                        {
                            [intf] = invoker
                        };
                        return invoker;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// ����һ������ֵʵ��
        /// </summary>
        protected virtual T CreateOutputInstance(Type type) =>
            (T)Activator.CreateInstance(type);

        /// <summary>
        /// �������
        /// </summary>
        public virtual Type OutputType { get; } = typeof(T);

        /// <summary>
        /// <seealso cref="OutputType"/>.FullName
        /// </summary>
        public virtual string TypeName { get; }

        /// <summary>
        /// <seealso cref="OutputType"/>���Ѻ�����
        /// </summary>
        public virtual string TypeFriendlyName { get; }

        /// <summary>
        /// ת������
        /// </summary>
        /// <param name="context">ת��������</param>
        /// <param name="input">����ֵ</param>
        /// <returns></returns>
        ConvertResult IConvertor.ChangeType(ConvertContext context, object input) =>
            ChangeTypeImpl(context, input);

        ConvertResult<T> ChangeTypeImpl(ConvertContext context, object input)
        {
            if (input is T t)
            {
                return new ConvertResult<T>(t);
            }
            //����쳣
            context.Exception = null;
            //��ֵת��
            if (input == null)
            {
                if (this is IFromNull<T> conv)
                {
                    var result = conv.FromNull(context);
                    return context.Exception ?? new ConvertResult<T>(result);
                }
                return context.InvalidCastException($"`null`{"�޷�ת��Ϊ"} {TypeFriendlyName:!}");
            }

            //��ȡָ���������͵�ת������������
            var invoker = GetInvoker(input.GetType());
            if (invoker != null)
            {
                var result = invoker.ChangeTypeImpl(this, context, input);
                if (result.Success)
                {
                    return result;
                }
                //��������쳣��,���滹���Գ�����������
            }
            else if (input is IEnumerator == false)
            {
                invoker = GetInvoker(typeof(IEnumerator));
                if (invoker != null)
                {
                    var ee = (input as IEnumerable)?.GetEnumerator()
                            ?? input as IEnumerator
                            ?? (input as DataTable)?.Rows.GetEnumerator()
                            ?? (input as DataRow)?.ItemArray.GetEnumerator()
                            ?? (input as DataRowView)?.Row.ItemArray.GetEnumerator()
                            ?? (input as IListSource)?.GetList()?.GetEnumerator();

                    var result = invoker.ChangeTypeImpl(this, context, ee);
                    if (result.Success)
                    {
                        return result;
                    }
                }
            }

            //תΪ���ֻ������ͽ���ת��,������ʧ���˾Ͳ��ټ�����
            if (input is IConvertible v0)
            {
                switch (v0.GetTypeCode())
                {
                    case TypeCode.DBNull:
                        if (this is IFromNull<T> conv)
                        {
                            var result = conv.FromDBNull(context);
                            return context.Exception ?? new ConvertResult<T>(result);
                        }
                        return context.Exception = new InvalidCastException(SR.GetString($"`DBNull`{"�޷�ת��Ϊ"} {TypeFriendlyName:!}"));
                    case TypeCode.Boolean:
                        return InvokeIForm(this, context, v0.ToBoolean(context.GetCultureInfo()));
                    case TypeCode.Byte:
                        return InvokeIForm(this, context, v0.ToByte(context.GetCultureInfo()));
                    case TypeCode.Char:
                        return InvokeIForm(this, context, v0.ToChar(context.GetCultureInfo()));
                    case TypeCode.DateTime:
                        return InvokeIForm(this, context, v0.ToDateTime(context.GetCultureInfo()));
                    case TypeCode.Decimal:
                        return InvokeIForm(this, context, v0.ToDecimal(context.GetCultureInfo()));
                    case TypeCode.Double:
                        return InvokeIForm(this, context, v0.ToDouble(context.GetCultureInfo()));
                    case TypeCode.Int16:
                        return InvokeIForm(this, context, v0.ToInt16(context.GetCultureInfo()));
                    case TypeCode.Int32:
                        return InvokeIForm(this, context, v0.ToInt32(context.GetCultureInfo()));
                    case TypeCode.Int64:
                        return InvokeIForm(this, context, v0.ToInt64(context.GetCultureInfo()));
                    case TypeCode.SByte:
                        return InvokeIForm(this, context, v0.ToSByte(context.GetCultureInfo()));
                    case TypeCode.Single:
                        return InvokeIForm(this, context, v0.ToSingle(context.GetCultureInfo()));
                    case TypeCode.String:
                        return InvokeIForm(this, context, v0.ToString(context.GetCultureInfo()));
                    case TypeCode.UInt16:
                        return InvokeIForm(this, context, v0.ToUInt16(context.GetCultureInfo()));
                    case TypeCode.UInt32:
                        return InvokeIForm(this, context, v0.ToUInt32(context.GetCultureInfo()));
                    case TypeCode.UInt64:
                        return InvokeIForm(this, context, v0.ToUInt64(context.GetCultureInfo()));
                    default:
                        break;
                }
            }

            //���ֿ�����string�ȼ۵�����,���Գ���תΪstring���ٴ�ת��
            if (input is StringBuilder
                || input is Uri
                || input is IPAddress
                || input is IFormattable)
            {
                var result = context.ChangeType<string>(input);
                if (result.Success)
                {
                    return InvokeIForm(this, context, result.OutputValue);
                }
                //����ʧ���� ��������objectת������
            }


            return InvokeIForm(this, context, input);
        }

        /// <summary>
        /// ����ת������
        /// </summary>
        /// <typeparam name="TInput">��������</typeparam>
        /// <param name="conv">ת����</param>
        /// <param name="context">ת��������</param>
        /// <param name="input">����ֵ</param>
        /// <returns></returns>
        static ConvertResult<T> InvokeIForm<TInput>(IConvertor<T> conv, ConvertContext context, TInput input)
        {
            try
            {

                if (conv is IFrom<TInput, T> from)
                {
                    var result = from.From(context, input);
                    if (context.Exception != null)
                    {
                        return context.Exception;
                    }
                    return new ConvertResult<T>(result);
                }
                if (input is IObjectReference refObj)
                {
                    var newObj = refObj.GetRealObject(new StreamingContext(StreamingContextStates.Clone, context));
                    if (!ReferenceEquals(newObj, refObj))
                    {
                        return conv.ChangeType(context, newObj);
                    }
                }
            }
            catch (Exception e)
            {
                return context.Exception = e;
            }

            return context.InvalidCastException(input, (conv as BaseConvertor<T>).TypeFriendlyName ?? conv.OutputType.GetFriendlyName());
        }

        /// <summary>
        /// ��ȡ��ת����
        /// </summary>
        /// <typeparam name="T1">��ת�����������</typeparam>
        /// <returns></returns>
        public virtual IConvertor<T1> GetConvertor<T1>()
        {
            var conv = GetConvertor(typeof(T1));
            if (conv == null)
            {
                return null;
            }
            return conv as IConvertor<T1> ?? new ProxyConvertor<T1>(conv);
        }

        /// <summary>
        /// ת��ת������
        /// </summary>
        /// <param name="context"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual ConvertResult<T> ChangeType(ConvertContext context, object input) =>
            ChangeTypeImpl(context, input);

        /// <summary>
        /// ��ȡ��ת����
        /// </summary>
        /// <param name="outputType">��ת�����������</param>
        /// <returns></returns>
        public virtual IConvertor GetConvertor(Type outputType) => null;

        /// <summary>
        /// ���ȼ� Ĭ��0
        /// </summary>
        public virtual uint Priority { get; } = 0;
    }
}