using HelperLibrary.Common;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using static HelperLibrary.Common.HidroAttributes;

namespace HelperLibrary {

    public static class HelperProviders {

        private static Random random = new Random();

        public static string GenerateTemporaryPassword() {
            const string CHARS = "QWERTYUIOPASDFGHJKKLZXCVBNMqwertyuiopasdfghjklzxcvbnmn1234567890!@#$%^&*_+.";
            var password = new string(
                Enumerable.Repeat(CHARS, 15)
                          .Select(p => p[random.Next(p.Length)])
                          .ToArray()
            );

            return password;
        }

        public static string GetValue(this Enum value) {

            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());

            StringValueAttribute[] atts = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false
            ) as StringValueAttribute[];

            return atts.Length > 0 ? atts[0].Value : null;
        }

        public static string CapitalizeFirstLetterOfEachWord(string sentence) {
            var newSentence = sentence.Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE)
                                      .ToLower();

            newSentence = Regex.Replace(newSentence, @"(^\w)|(\s\w)", m => m.Value.ToUpper());

            return newSentence;
        }
    }
}
