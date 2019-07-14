using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Kanai
{
    internal abstract class Convertor
    {
        /// <summary>
        /// 返回指定类型的对象，其值等效于指定对象。
        /// </summary>
        /// <param name="context"> 上下文 </param>
        /// <param name="input"> 需要转换类型的对象 </param>
        protected abstract ConvertResult<object> ChangeType(ConvertContext context, object input);
    }
}
