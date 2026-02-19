using Session.Scheme.Variables;
using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, string> _variableTypeAliases = new Dictionary<Type, string>
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

        private static readonly Dictionary<VariableService.ActionOperatorType, string> _actionOperatorTypeAliases = new()
        {
            { VariableService.ActionOperatorType.FirstEqualSecond, "=" },
            { VariableService.ActionOperatorType.FirstPlusSecond, "+=" },
            { VariableService.ActionOperatorType.FirstMinusSecond, "-=" },
            { VariableService.ActionOperatorType.FirstDividedBySecond, "/=" },
            { VariableService.ActionOperatorType.FirstMultypliedBySecond, "*=" },
        };

        private static readonly Dictionary<VariableService.ConditionOperatorType, string> _conditionOperatorTypeAliases = new()
        {
            { VariableService.ConditionOperatorType.IsEqual, "==" },
            { VariableService.ConditionOperatorType.IsNotEqual, "!=" },
            { VariableService.ConditionOperatorType.IsGreater, ">" },
            { VariableService.ConditionOperatorType.IsGreaterOrEqual, ">=" },
            { VariableService.ConditionOperatorType.IsLess, "<" },
            { VariableService.ConditionOperatorType.IsLessOrEqual, "<=" },
        };

        public static string GetFriendlyTypeName(this Type type)
        {
            if (_variableTypeAliases.TryGetValue(type, out string alias))
                return alias;
            else
                return type.Name;
        }

        public static string GetFriendlyActionOperatorTypeName(this VariableService.ActionOperatorType type)
        {
            if (_actionOperatorTypeAliases.TryGetValue(type, out string alias))
                return alias;
            else
                throw new NullReferenceException($"There is no value with type: {type}");
        }

        public static string GetFriendlyConditionOperatorTypeName(this VariableService.ConditionOperatorType type)
        {
            if (_conditionOperatorTypeAliases.TryGetValue(type, out string alias))
                return alias;
            else
                throw new NullReferenceException($"There is no value with type: {type}");
        }
    }
}