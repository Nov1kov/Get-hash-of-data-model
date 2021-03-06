using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ClassHashCode
{
    public class ClassHashCode
    {
        private ISet<Type> _checkedTypes = new HashSet<Type>();
        private readonly Type _type;
        private int _deep;

        private int _hashCode;

        private ClassHashCode(Type type)
        {
            _type = type;
        }

        public static int Get(Type type)
        {
            var hashCoder = new ClassHashCode(type);
            hashCoder.Execute();
            return hashCoder._hashCode;
        }
        
        private void Execute()
        {
            Debug.WriteLine($"Getting hashCode of type: {_type.Name}");
            CombineWithType(_type, false);
        }

        private void CombineWithType(Type type, bool findSubClasses)
        {
            _deep += 1;
            Debug.WriteLine(new string('\t', _deep) + $"type: {type.Name}");

            if (IsTheSameType(type))
            {
                _deep -= 1;
                return;
            }
            
            var fullName = type.FullName;
            CombineHashCode(GetDeterministicHashCode(fullName));
            
            var subClasses = AppDomain.CurrentDomain.GetAssemblies().
                SelectMany(s => s.GetTypes()).
                Where(t => t.BaseType == type || t.GetInterfaces().Contains(type) && !t.IsInterface).ToList();
            if (findSubClasses && subClasses.Any())
            {
                foreach (var subClass in subClasses)
                {
                    Console.WriteLine($"Found subclass for {type} - {subClass}");
                    CombineWithType(subClass, true);
                }
            }

            if (type.IsGenericType)
            {
                foreach (var genericArgument in type.GetGenericArguments())
                {
                    CombineWithType(genericArgument, true);
                }   
            }
            
            // is only collections interfaces?
            if (type.IsInterface)
            {
                _deep -= 1;
                return;
            }
            
            var fields = GetFields(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .ToList();

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute(typeof(NonSerializedAttribute)) != null)
                {
                    continue;
                }

                var fieldType = GetFieldType(field);
                
                var nameField = field.Name;
                Debug.WriteLine(new string('\t', _deep) + $"\tfield: {nameField}");
                CombineHashCode(GetDeterministicHashCode(nameField));
                
                if (!IsPrimitiveType(fieldType))
                {
                    CombineWithType(fieldType, true);
                }
            }

            _deep -= 1;
            return;
        }

        private static Type GetFieldType(FieldInfo field)
        {
            var fieldType = field.FieldType;

            if (fieldType.GetCustomAttribute(typeof(SerializableAttribute)) == null && !fieldType.IsInterface)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Not serializable attribute for type: {fieldType}, in field: {field.Name}");
                Console.ResetColor();
            }

            return fieldType;
        }
        
        private bool IsTheSameType(Type type)
        {
            if (_checkedTypes.Contains(type))
            {
                return true;
            }
            
            _checkedTypes.Add(type);
            return false;
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

        private static bool IsPrimitiveType(Type fieldType)
        {
            return fieldType.IsPrimitive || fieldType == typeof(string) || fieldType == typeof(decimal) || fieldType.IsEnum;
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