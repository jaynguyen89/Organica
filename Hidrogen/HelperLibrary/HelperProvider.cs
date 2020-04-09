using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HelperLibrary.Common;
using static HelperLibrary.Common.HidroAttributes;

namespace HelperLibrary {

    public static class HelperProvider {

        private static Random random = new Random();

        public static string GenerateRandomString(int length, bool includeSpecialChars = true) {
            const string SCHARS = "QWERTYUIOPASDFGHJKKLZXCVBNMqwertyuiopasdfghjklzxcvbnmn1234567890!@#$%^&*_+.";
            const string NCHARS = "QWERTYUIOPASDFGHJKKLZXCVBNMqwertyuiopasdfghjklzxcvbnmn";
            var password = new string(
                Enumerable.Repeat(includeSpecialChars ? SCHARS : NCHARS, length)
                          .Select(p => p[random.Next(p.Length)])
                          .ToArray()
            );

            return password;
        }

        public static int RandomNumberInRange(int min, int max) {
            Random rand = new Random();
            return rand.Next(min, max);
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

        public static bool IsNumber(string any) {
            return int.TryParse(any, out _);
        }

        public static DateTime? ParseDateTimeString(string datetime, string format) {
            DateTime dt;
            
            if (DateTime.TryParseExact(datetime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;

            return null;
        }
    }
}
