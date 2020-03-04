using System;
using System.Reflection;
using static HelperLibrary.Common.HidroAttributes;

namespace HelperLibrary {

    public static class HelperProviders {

        public static string GetStringValue(this Enum value) {

            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());

            StringValueAttribute[] atts = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false
            ) as StringValueAttribute[];

            return atts.Length > 0 ? atts[0].Value : null;
        }
    }
}
