using System;
using System.Collections.Generic;
using System.Linq;

namespace LitJson
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class JsonPropertyNameAttribute : Attribute
    {
        public string Name
        {
            get;
        }

        public JsonPropertyNameAttribute(string name)
        {
            Name = name;
        }
    }
}
