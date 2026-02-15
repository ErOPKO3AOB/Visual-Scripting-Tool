using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, string> _aliases = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(object), "object" },
            { typeof(string), "string" }
        };

        public static string GetFriendlyName(this Type type)
        {
            if (_aliases.TryGetValue(type, out string alias))
                return alias;

            // Для ненулевых значимых типов можно обработать Nullable<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = type.GetGenericArguments()[0];
                return underlyingType.GetFriendlyName() + "?";
            }

            // Для остальных типов возвращаем обычное имя (можно добавить обработку вложенных/обобщённых)
            return type.Name;
        }
    }
}