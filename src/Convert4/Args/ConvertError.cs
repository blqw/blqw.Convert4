//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace blqw
//{
//    /// <summary>
//    /// 转换错误实体
//    /// </summary>
//    public sealed class ConvertError
//    {
//        /// <summary>
//        /// 初始化异常
//        /// </summary>
//        public ConvertError(Exception exception) => AddException(exception);

//        /// <summary>
//        /// 异常消息
//        /// </summary>
//        /// <returns>返回用户设置的message值,如果没有指定,则返回第一个Exception的Message</returns>
//        public string Message =>
//            Exceptions.LastOrDefault()?.Message;

//        private readonly List<Exception> _exceptions = new List<Exception>();
//        /// <summary>
//        /// 异常集合
//        /// </summary>
//        /// <returns></returns>
//        public IEnumerable<Exception> Exceptions => _exceptions;

//        public void AddException(Exception exception)
//        {
//            if (exception == null || _exceptions.Contains(exception))
//            {
//                return;
//            }
//            if (exception is AggregateException aggregateException)
//            {
//                foreach (var ex in aggregateException.InnerExceptions)
//                {
//                    AddException(ex);
//                }
//                return;
//            }
//            _exceptions.Add(exception);
//        }

//        public void AddError(ConvertError error)
//        {
//            foreach (var ex in error._exceptions)
//            {
//                AddException(ex);
//            }
//        }

//        /// <summary>
//        /// 如果没有异常返回null <para />
//        /// 如果只有一个异常返回具体异常  <para />
//        /// 如果多余一个异常返回的集合 <seealso cref="AggregateException"/>
//        /// </summary>
//        public Exception Exception
//        {
//            get
//            {
//                if (_exceptions.Count == 0)
//                {
//                    return null;
//                }
//                return new AggregateException(Message, _exceptions);
//            }
//        }

//        /// <summary>
//        /// 如果有异常则抛出异常, 否则不执行任何操作
//        /// </summary>
//        public void TryThrow()
//        {
//            var ex = Exception;
//            if (ex != null)
//            {
//                throw ex;
//            }
//        }


//        internal Scope CreateScope() => new Scope(this);

//        private int _scopeMark = 0;

//        public struct Scope : IDisposable
//        {
//            private readonly ConvertError _convertError;
//            private readonly int _prevMark;

//            public Scope(ConvertError convertError)
//            {
//                _convertError = convertError;
//                _prevMark = convertError._scopeMark;
//                _convertError._scopeMark = convertError._exceptions.Count;
//            }

//            public bool HasError => _convertError._scopeMark < _convertError._exceptions.Count;

//            public void Dispose() => _convertError._scopeMark = _prevMark;
//        }

//        public void Clear()
//        {
//            if (_scopeMark == 0)
//            {
//                _exceptions.Clear();
//            }
//            else
//            {
//                _exceptions.RemoveRange(_scopeMark, _exceptions.Count - _scopeMark);
//            }
//        }

//        #region 运算符重载
//        public static Exception operator +(Exception ex, ConvertError error)
//        {
//            if (ex == null || error?.Exceptions == null)
//            {
//                return ex ?? error?.Exception;
//            }

//            if (error._exceptions.Count == 0)
//            {
//                return ex;
//            }
//            if (error._exceptions.Count == 1)
//            {
//                return new AggregateException(ex.Message ?? error.Message, new Exception[] { ex, error._exceptions[0] });
//            }
//            var errors = new Exception[error._exceptions.Count + 1];
//            error._exceptions.CopyTo(errors, 1);
//            errors[0] = ex;
//            return new AggregateException(ex.Message, errors.Reverse());
//        }


//        #endregion

//        public ConvertError Clone()
//        {
//            var error = new ConvertError(null);
//            error._exceptions.AddRange(_exceptions);
//            return error;
//        }
//    }
//}