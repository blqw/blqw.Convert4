using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace blqw
{

    [Serializable]
    public class ConvertException : Exception
    {
        public ConvertException(Exception inner) : base(inner?.Message, (inner as ConvertException)?.InnerException ?? inner) =>
            _exceptions = (inner as ConvertException)?._exceptions;
        protected ConvertException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        private List<Exception> _exceptions;
        /// <summary>
        /// 异常集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Exception> InnerExceptions
        {
            get
            {
                if (InnerException != null)
                {
                    yield return InnerException;
                }
                if (_exceptions != null && _exceptions.Count > 0)
                {
                    for (var i = _exceptions.Count - 1; i >= 0; i--)
                    {
                        yield return _exceptions[i];
                    }
                }
            }
        }

        public ConvertException AddException(Exception exception)
        {
            if (exception == null)
            {
                return this;
            }
            switch (exception)
            {
                case AggregateException ex:
                    foreach (var e in ex.InnerExceptions)
                    {
                        AddException(e);
                    }
                    return this;
                case ConvertException ex:
                    if (ex != this)
                    {
                        if (ex._exceptions != null && ex._exceptions.Count > 0)
                        {
                            for (var i = 0; i < ex._exceptions.Count; i++)
                            {
                                AddException(ex._exceptions[i]);
                            }
                        }
                        if (InnerException != null)
                        {
                            AddException(ex.InnerException);
                        }
                    }
                    return this;
                default:
                    var e1 = exception.GetBaseException();
                    if (e1 == exception)
                    {
                        if (_exceptions == null)
                        {
                            _exceptions = new List<Exception>();
                        }
                        _exceptions.Add(exception);
                    }
                    else
                    {
                        AddException(e1);
                    }
                    return this;
            }
        }

        internal void TryThrow()
        {
            if (ExceptionCount > 0)
            {
                throw this;
            }
        }

        public override string Message
        {
            get
            {
                if (_exceptions == null || _exceptions.Count == 0)
                {
                    return base.Message ?? "没有错误";
                }
                return _exceptions[0].Message;
            }
        }

        public string Messages => string.Join(Environment.NewLine, InnerExceptions.Select(x => x.Message));

        internal int ExceptionCount => (_exceptions?.Count ?? 0) + (InnerException == null ? 0 : 1);

        #region 运算符重载
        public static ConvertException operator +(ConvertException ex1, Exception ex2)
        {
            if (ex1 != null)
            {
                return ex1.AddException(ex2);
            }
            if (ex2 != null)
            {
                return ex2 as ConvertException ?? new ConvertException(ex2);
            }
            return null;
        }

        #endregion
    }
}
