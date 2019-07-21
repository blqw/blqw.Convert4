using blqw.Kanai.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blqw.Kanai.Convertors
{
    internal sealed class AggregateConvertor<T> : IConvertor<T>
    {
        public AggregateConvertor(IEnumerable<IConvertor<T>> convertors)
        {
            if (convertors == null)
            {
                throw new ArgumentNullException(nameof(convertors));
            }

            Convertors = convertors.Where(x => !(x is AggregateConvertor<T>)).Union(
                            convertors.OfType<AggregateConvertor<T>>().SelectMany(x => x.Convertors)
                         ).OrderBy(x => x.Priority).ToList().AsReadOnly();
            if (Convertors.Count < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(convertors), "转换器必须大于1个");
            }
        }

        public uint Priority => 100;

        public IReadOnlyCollection<IConvertor<T>> Convertors { get; }

        ConvertResult<T> IConvertor<T>.ChangeType(ConvertContext context, object input)
        {
            List<Exception> exceptions = null;
            foreach (var convertor in Convertors)
            {
                var result = convertor.ChangeType(context, input);
                if (result.Success)
                {
                    return result;
                }
                if (exceptions == null)
                {
                    exceptions = new List<Exception>() { result.Exception };
                }
                else
                {
                    exceptions.Add(result.Exception);
                }
            }
            return this.Fail(input, context.CultureInfo, exceptions);
        }

        //protected override ConvertResult<object> ChangeType(ConvertContext context, object input)
        //{
        //    var result = ChangeType(context, input);
        //    return new ConvertResult<object>(result.Success, result.OutputValue, result.Exception);
        //}
    }
}
