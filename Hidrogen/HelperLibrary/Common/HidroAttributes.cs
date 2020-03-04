using System;

namespace HelperLibrary.Common {

    class HidroAttributes {

        public sealed class StringValueAttribute : Attribute {

            public string Value { get; set; }

            public StringValueAttribute(string v) { Value = v; }
        }
    }
}
