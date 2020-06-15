using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HelperLibrary.ArrayExtensions;
using HelperLibrary.Common;
using static HelperLibrary.Common.HidroAttributes;

namespace HelperLibrary {

    public static class HelperProvider {

        private static readonly Random Random = new Random();
        private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        public static string GenerateRandomString(int length, bool includeSpecialChars = true) {
            const string SCHARS = "QWERTYUIOPASDFGHJKKLZXCVBNMqwertyuiopasdfghjklzxcvbnmn1234567890!@#$%^&*_+.";
            const string NCHARS = "QWERTYUIOPASDFGHJKKLZXCVBNMqwertyuiopasdfghjklzxcvbnmn";
            var password = new string(
                Enumerable.Repeat(includeSpecialChars ? SCHARS : NCHARS, length)
                          .Select(p => p[Random.Next(p.Length)])
                          .ToArray()
            );

            return password;
        }

        public static int RandomNumberInRange(int min, int max) {
            var rand = new Random();
            return rand.Next(min, max);
        }

        public static string GetValue(this Enum value) {

            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());

            if (fieldInfo != null) {
                return fieldInfo.GetCustomAttributes(
                    typeof(StringValueAttribute), false
                ) is StringValueAttribute[] attributes && attributes.Length > 0 ? attributes[0].Value : null;
            }

            return string.Empty;
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

        private static bool IsPrimitive(this Type type) {
            if (type == typeof(string)) return true;
            return (type.IsValueType & type.IsPrimitive);
        }

        private static object Copy(this object originalObject) {
            return InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
        }
        
        private static object InternalCopy(object originalObject, IDictionary<object, object> visited) {
            if (originalObject == null) return null;
            
            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect)) return originalObject;
            
            if (visited.ContainsKey(originalObject)) return visited[originalObject];
            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
            
            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray) {
                var arrayType = typeToReflect.GetElementType();

                if (!IsPrimitive(arrayType)) {
                    var clonedArray = (Array) cloneObject;
                    if (clonedArray == null) throw new ArgumentNullException(nameof(clonedArray));

                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }
            }
            
            visited.Add(originalObject, cloneObject);
            
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(
            object originalObject,
            IDictionary<object, object> visited,
            object cloneObject,
            Type typeToReflect
        ) {
            if (typeToReflect.BaseType == null) return;
            
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
            CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
        }

        private static void CopyFields(
            object originalObject,
            IDictionary<object, object> visited,
            object cloneObject,
            IReflect typeToReflect,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy,
            Func<FieldInfo, bool> filter = null
        ) {
            foreach (var fieldInfo in typeToReflect.GetFields(bindingFlags)) {
                if (filter != null && filter(fieldInfo) == false) continue;
                if (IsPrimitive(fieldInfo.FieldType)) continue;
                
                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }
        
        public static T Copy<T>(this T original) {
            return (T)Copy((object)original);
        }
    }

    public class ReferenceEqualityComparer : EqualityComparer<object> {
        
        public override bool Equals(object x, object y) {
            return ReferenceEquals(x, y);
        }
        
        public override int GetHashCode(object obj) {
            return obj.GetHashCode();
        }
    }

    namespace ArrayExtensions {
        
        public static class ArrayExtensions {
            
            public static void ForEach(this Array array, Action<Array, int[]> action) {
                if (array.LongLength == 0) return;
                
                var walker = new ArrayTraverse(array);
                
                do action(array, walker.Position);
                while (walker.Step());
            }
        }

        internal class ArrayTraverse {
            public readonly int[] Position;
            private readonly int[] _maxLengths;

            public ArrayTraverse(Array array) {
                _maxLengths = new int[array.Rank];
                
                for (var i = 0; i < array.Rank; ++i)
                    _maxLengths[i] = array.GetLength(i) - 1;
                
                Position = new int[array.Rank];
            }

            public bool Step() {
                for (var i = 0; i < Position.Length; ++i) {
                    if (Position[i] >= _maxLengths[i]) continue;
                    
                    Position[i]++;
                    
                    for (var j = 0; j < i; j++)
                        Position[j] = 0;
                    
                    return true;
                }
                
                return false;
            }
        }
    }
}
