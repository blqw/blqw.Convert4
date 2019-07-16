using blqw.Kanai.Convertors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace blqw.Kanai
{
    /// <summary>
    /// 转换器选择器
    /// </summary>
    internal class ConvertorSelector : IConvertorSelector
    {
        private readonly IEnumerable<IConvertorFactory> _factories;

        public ConvertorSelector(IEnumerable<IConvertorFactory> factories) =>
            _factories = factories ?? throw new ArgumentNullException(nameof(factories));

        public IConvertor<T> GetConvertor<T>(ConvertContext context)
        {
            var factories = _factories.Where(x => x.CanBuild<T>()).ToList();
            switch (factories.Count)
            {
                case 0:
                    return null;
                case 1:
                    return factories[0].Build<T>();
                default:
                    return new AggregateConvertor<T>(factories.Select(x => x.Build<T>()));
            }
        }
    }
}
