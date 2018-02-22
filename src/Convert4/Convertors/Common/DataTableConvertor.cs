using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace blqw.Convertors
{
    /// <summary>
    /// <seealso cref="DataTable" /> 转换器
    /// </summary>
    class DataTableConvertor : BaseConvertor<DataTable>
                             , IFrom<DataView, DataTable>
                             , IFrom<IDataReader, DataTable>
                             , IFrom<IEnumerator, DataTable>
    {
        public DataTable From(ConvertContext context, DataView input) => input?.ToTable();

        public DataTable From(ConvertContext context, IDataReader input)
        {
            if (input == null)
            {
                return null;
            }
            if (input.IsClosed)
            {
                context.Exception = new InvalidOperationException(SR.GetString($"DataReader {"已经关闭"}"));
                return null;
            }
            var table = new DataTable();
            table.Load(input);
            return table;
        }

        public DataTable From(ConvertContext context, IEnumerator input)
        {

            var ee = input;

            if (ee == null)
            {
                context.AddException("仅支持DataView,DataRow,DataRowView,或实现IEnumerator,IEnumerable,IListSource,IDataReader接口的对象对DataTable的转换");
                success = false;
                return null;
            }
            var builder = new DataTableBuilder(context);
            builder.TryCreateInstance();
            while (ee.MoveNext())
            {
                if (builder.Set(ee.Current) == false)
                {
                    success = false;
                    return null;
                }
            }
            return builder.Instance;
        }

        /// <summary>
        /// <seealso cref="DataTable" /> 构造器
        /// </summary>
        private struct DataTableBuilder
        {
            /// <summary>
            /// 转换上下文
            /// </summary>
            private readonly ConvertContext _context;

            /// <summary>
            ///     <seealso cref="DataTable.Columns" />
            /// </summary>
            private DataColumnCollection _columns;

            /// <summary>
            /// 初始化构造器
            /// </summary>
            /// <param name="context"> 转换上下文 </param>
            public DataTableBuilder(ConvertContext context)
            {
                _context = context;
                _columns = null;
                Instance = null;
            }

            /// <summary>
            /// 设置对象值
            /// </summary>
            /// <param name="value"> 待设置的值 </param>
            /// <returns> </returns>
            public bool Set(object value)
            {
                var mapper = new Mapper(value);
                if (mapper.Error != null)
                {
                    _context.AddException(mapper.Error);
                    return false;
                }
                var row = Instance.NewRow();

                while (mapper.MoveNext())
                {
                    var name = mapper.Key as string;
                    if (name == null)
                    {
                        _context.AddException("标题必须为字符串");
                        return false;
                    }
                    if (AddCell(row, name, typeof(object), mapper.Value) == false)
                    {
                        return false;
                    }
                }
                Instance.Rows.Add(row);
                return true;
            }

            /// <summary>
            /// 添加单元格中的值
            /// </summary>
            /// <param name="row"> 需要添加单元格的行 </param>
            /// <param name="name"> 单元格列名 </param>
            /// <param name="type"> 值类型 </param>
            /// <param name="value"> 需要添加的值 </param>
            /// <returns> </returns>
            private bool AddCell(DataRow row, string name, Type type, object value)
            {
                var col = _columns[name];
                if (col == null)
                {
                    col = _columns.Add(name, type);
                }
                else if (col.DataType != type)
                {
                    bool success;
                    value = value.ChangeType(col.DataType, out success);
                    if (success == false)
                    {
                        _context.AddException($"第{Instance?.Rows.Count}行{col.ColumnName}列添加到行失败");
                        return false;
                    }
                }
                row[col] = value;
                return true;
            }

            /// <summary>
            /// 被构造的实例
            /// </summary>
            public DataTable Instance { get; private set; }

            /// <summary>
            /// 尝试构造实例,返回是否成功
            /// </summary>
            /// <returns> </returns>
            public bool TryCreateInstance()
            {
                Instance = new DataTable();
                _columns = Instance.Columns;
                return true;
            }
        }   



    }
}
