using blqw.Kanai.Extensions;
using blqw.Kanai.Interface.From;
using System;

namespace blqw.Kanai.Convertors
{
    /// <summary>
    /// 布尔值转换器
    /// </summary>
    public class BooleanConvertor : BaseConvertor<bool>, IFromConvertible<bool>, IFrom<object, bool>
    {
        public BooleanConvertor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ConvertResult<bool> From(ConvertContext context, bool input) => input;
        public ConvertResult<bool> From(ConvertContext context, char input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, sbyte input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, byte input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, short input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, ushort input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, int input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, uint input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, long input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, ulong input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, float input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, double input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, decimal input) => input != 0;
        public ConvertResult<bool> From(ConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
            switch (s.Length)
            {
                case 1:
                    switch (s[0])
                    {
                        case '1':
                        case 'T':
                        case 't':
                        case '对':
                        case '對':
                        case '真':
                        case '是':
                        case '男':
                            return true;
                        case '0':
                        case 'F':
                        case 'f':
                        case '错':
                        case '錯':
                        case '假':
                        case '否':
                        case '女':
                            return false;
                        default:
                            break;
                    }
                    break;
                case 2:
                    if ((s[0] == '-') || (s[0] == '+'))
                    {
                        return s[1] != '0';
                    }
                    break;
                case 4:
                    if (s.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    break;
                case 5:
                    if (s.Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }
            return this.Fail(context, input);
        }
        public ConvertResult<bool> From(ConvertContext context, object input) => this.Fail(context, input);
        public ConvertResult<bool> From(ConvertContext context, DateTime input) => this.Fail(context, input);
    }
}
