using System;

namespace Chinese.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AbbreviationAttribute : Attribute
    {
        public string Value { get; private set; }

        public AbbreviationAttribute(string value)
        {
            Value = value;
        }
    }
}
