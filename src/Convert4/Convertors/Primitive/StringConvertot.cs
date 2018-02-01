using blqw.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Convertors
{
    class StringConvertot : BaseConvertor<string>, IFrom<string, Type>
    {
        public string From(ConvertContext context, Type input) =>
            input.GetFriendlyName();
    }
}
