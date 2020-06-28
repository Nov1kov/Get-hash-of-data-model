using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ClassHashCode
{
    public class ClassHashCode
    {
        private readonly Type _type;
        private const int ArrayConstHashModifier = -1254979452;

        private int _hashCode;

        private ClassHashCode(Type type)
        {
            _type = type;
        }

        public static int Get(Type type)
        {
            var hashCoder = new ClassHashCode(type);
            return hashCoder.Execute();
        }

        private int Execute()
        {
            var props = GetFields(_type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .ToList();

            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute(typeof(NonSerializedAttribute)) != null)
                {
                    continue;
                }
                
                var propType = prop.FieldType;
                
                var isArray = propType.IsArray;
                if (isArray)
                {
                    propType = propType.GetElementType();
                    if (propType == null)
                    {
                        throw new Exception($"Wrong array without element: {prop.FieldType}");
                    }
                }

                if (propType.GetCustomAttribute(typeof(SerializableAttribute)) == null)
                {
                    throw new Exception($"Not serializable attribute: {propType}");
                }

                if (IsDefaultSerializable(propType))
                {
                    var nameField = prop.Name;
                    CombineHashCode(GetDeterministicHashCode(nameField));
                }
                else
                {
                    CombineHashCode(Get(propType));
                }

                if (isArray)
                {
                    CombineHashCode(ArrayConstHashModifier);
                }
            }

            return _hashCode;
        }

        private void CombineHashCode(int hash)
        {
            unchecked
            {
                _hashCode = _hashCode * 31 + hash;
            }
        }
        
        // https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
        private static int GetDeterministicHashCode(string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        private static bool IsDefaultSerializable(Type propType)
        {
            return propType.IsPrimitive || propType == typeof(string) || propType.IsEnum;
        }
        
        private static IEnumerable<FieldInfo> GetFields(
            Type targetType,
            BindingFlags bindingAttr)
        {
            List<MemberInfo> source = new List<MemberInfo>(targetType.GetFields(bindingAttr));
            GetChildFields(source, targetType, bindingAttr);
            return source.Cast<FieldInfo>();
        }
    
        private static void GetChildFields(
            IList<MemberInfo> initialFields,
            Type targetType,
            BindingFlags bindingAttr)
        {
            while ((targetType = targetType.BaseType) != null)
            {
                var fieldInfos = targetType.GetFields(bindingAttr);
                foreach (var fieldInfo in fieldInfos)
                {
                    initialFields.Add(fieldInfo);
                }
            }
        }
    }
}