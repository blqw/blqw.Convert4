namespace blqw.Kanai
{
    public class StringSeparator
    {
        string _str;

        public StringSeparator(char value)
        {
            _str = value.ToString();
            FirstChar = value;
            CharArray = new[] { value };
        }

        public StringSeparator(char[] value)
        {
            if (value == null || value.Length == 0)
            {
                value = new[] { ',' };
            }

            _str = new string(value);
            FirstChar = value[0];
            CharArray = value;
        }

        private StringSeparator(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = ",";
            }
            _str = value;
            FirstChar = value[0];
            CharArray = value.ToCharArray();
        }

        public char FirstChar { get; }
        public char[] CharArray { get; }

        public override string ToString() => _str;

        public static implicit operator StringSeparator(string value) => new StringSeparator(value);
        public static implicit operator StringSeparator(char value) => new StringSeparator(value);
        public static implicit operator StringSeparator(char[] value) => new StringSeparator(value);

        public static implicit operator char[](StringSeparator value) => value?.CharArray ?? new[] { ',' };
        public static implicit operator char(StringSeparator value) => value?.FirstChar ?? ',';
        public static implicit operator string(StringSeparator value) => value?.ToString() ?? ",";

    }
}
