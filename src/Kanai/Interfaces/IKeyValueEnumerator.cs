using System.Collections.Generic;

namespace blqw.Kanai.Interface
{
    /// <summary>
    /// 提供获取当前元素键值方法的枚举器
    /// </summary>
    internal interface IKeyValueEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// 获取集合中的当前元素的Key
        /// </summary>
        /// <returns> </returns>
        ConvertResult<TKey> GetKey();

        /// <summary>
        /// 获取集合中的当前元素的Value
        /// </summary>
        /// <returns> </returns>
        ConvertResult<TValue> GetValue();
    }
}
