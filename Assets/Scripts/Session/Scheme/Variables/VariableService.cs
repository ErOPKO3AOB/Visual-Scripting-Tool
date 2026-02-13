using System;
using System.Collections.Generic;
using UnityEngine;

namespace Session.Scheme.Variables
{
    public class VariableService
    {
        public enum MethodOperatorType
        {
            [InspectorName("=")]
            FirstEqualSecond,

            [InspectorName("+=")]
            FirstPlusSecond,

            [InspectorName("-=")]
            FirstMinusSecond,

            [InspectorName("/=")]
            FirstDividedBySecond,

            [InspectorName("*=")]
            FirstMultypliedBySecond
        }

        public enum ConditionOperatorType
        {
            [InspectorName("==")]
            IsEqual,

            [InspectorName("!=")]
            IsNotEqual,

            [InspectorName(">")]
            IsGreater,

            [InspectorName(">=")]
            IsGreaterOrEqual,

            [InspectorName("<")]
            IsLess,

            [InspectorName("<=")]
            IsLessOrEqual,
        }

        public List<SchemeVariableBase> Variables { get; private set; } = new List<SchemeVariableBase>();

        #region Variable Creating
        public void BuildVariable<T>(string varName, T startValue = default)
        {
            if (string.IsNullOrEmpty(varName)) return;

            int index = CheckVariableExistance(varName);

            if (index < 0)
            {
                SchemeVariableBase schemeVariable = new SchemeVariable<T>(varName);
                Variables.Add(schemeVariable);
            }
            Debug.Log($"int {varName} vairalble with value:{startValue}");

            SetValueToVariable(varName, startValue);
        }

        public void SetTypeToVariable<T>(string varName)
        {
            int index = CheckVariableExistance(varName);

            if (index > -1)
            {
                SchemeVariableBase schemeVariable = Variables[index];
                RemoveVariable(schemeVariable.variableName);
                BuildVariable<T>(schemeVariable.variableName);
            }
        }

        public void SetValueToVariable(string varName, object value)
        {
            int index = CheckVariableExistance(varName);

            if (index > -1)
            {
                Variables[index].SetValue(value);
            }
        }

        public void RemoveVariable(string varName)
        {
            int index = CheckVariableExistance(varName);

            if (index > -1)
            {
                Variables.RemoveAt(index);
            }
        }

        public int CheckVariableExistance(string varName)
        {
            if (Variables.Count > 0)
                for (int i = 0; i < Variables.Count; i++)
                    if (Variables[i].variableName == varName)
                        return i;

            return -1;
        }

        public int GetTypeIntegerValue(Type type)
        {
            int typeValue = 0;

            switch (type)
            {
                case Type intType when intType == typeof(int):
                    typeValue = 0;
                    break;
                case Type floatType when floatType == typeof(float):
                    typeValue = 1;
                    break;
                case Type stringType when stringType == typeof(string):
                    typeValue = 2;
                    break;
                case Type boolType when boolType == typeof(bool):
                    typeValue = 3;
                    break;
            }

            return typeValue;
        }
        #endregion

        #region Variable Operating
        public void UseOperation(string operand1Name, MethodOperatorType opType, string operand2Name)
        {
            SchemeVariableBase operand1 = Variables[CheckVariableExistance(operand1Name)];
            SchemeVariableBase operand2 = Variables[CheckVariableExistance(operand2Name)];

            if ((operand1.ValueType != operand2.ValueType) || (operand1 == null || operand2 == null))
            {
                Debug.Log("Value types exception!");
                return; // throw Exception();
            }

            if (operand1.ValueType == typeof(int) && operand2.ValueType == typeof(int))
            {
                operand1 = (SchemeVariable<int>)operand1;
                operand2 = (SchemeVariable<int>)operand2;

                switch (opType)
                {
                    case MethodOperatorType.FirstEqualSecond:
                        operand1.SetValue((int)operand2.GetValue());
                        break;
                    case MethodOperatorType.FirstPlusSecond:
                        operand1.SetValue((int)operand1.GetValue() + (int)operand2.GetValue());
                        break;
                    case MethodOperatorType.FirstMinusSecond:
                        operand1.SetValue((int)operand1.GetValue() - (int)operand2.GetValue());
                        break;
                    case MethodOperatorType.FirstDividedBySecond:
                        operand1.SetValue((int)operand1.GetValue() / (int)operand2.GetValue());
                        break;
                    case MethodOperatorType.FirstMultypliedBySecond:
                        operand1.SetValue((int)operand1.GetValue() * (int)operand2.GetValue());
                        break;
                }
            }

            else if (operand1.ValueType == typeof(float) && operand2.ValueType == typeof(float))
            {
                operand1 = (SchemeVariable<float>)operand1;
                operand2 = (SchemeVariable<float>)operand2;

                switch (opType)
                {
                    case MethodOperatorType.FirstEqualSecond:
                        operand1.SetValue((float)operand2.GetValue());
                        break;
                    case MethodOperatorType.FirstPlusSecond:
                        operand1.SetValue((float)operand1.GetValue() + (float)operand2.GetValue());
                        break;
                    case MethodOperatorType.FirstMinusSecond:
                        operand1.SetValue((float)operand1.GetValue() - (float)operand2.GetValue());
                        break;
                    case MethodOperatorType.FirstDividedBySecond:
                        operand1.SetValue((float)operand1.GetValue() / (float)operand2.GetValue());
                        break;
                    case MethodOperatorType.FirstMultypliedBySecond:
                        operand1.SetValue((float)operand1.GetValue() * (float)operand2.GetValue());
                        break;
                }
            }

            else if (operand1.ValueType == typeof(string) && operand2.ValueType == typeof(string))
            {
                operand1 = (SchemeVariable<string>)operand1;
                operand2 = (SchemeVariable<string>)operand2;

                switch (opType)
                {
                    case MethodOperatorType.FirstEqualSecond:
                        operand1.SetValue((string)operand2.GetValue());
                        break;
                    case MethodOperatorType.FirstPlusSecond:
                        operand1.SetValue((string)operand1.GetValue() + (string)operand2.GetValue());
                        break;
                        //case OperatorType.FirstMinusSecond:
                        //    operand1.SetValue((string)operand1.GetValue() - (string)operand2.GetValue());
                        //    break;
                        //case OperatorType.FirstDividedBySecond:
                        //    operand1.SetValue((string)operand1.GetValue() / (string)operand2.GetValue());
                        //    break;
                        //case OperatorType.FirstMultypliedBySecond:
                        //    operand1.SetValue((string)operand1.GetValue() * (string)operand2.GetValue());
                        //    break;
                }
            }

            else if (operand1.ValueType == typeof(bool) && operand2.ValueType == typeof(bool))
            {
                operand1 = (SchemeVariable<bool>)operand1;
                operand2 = (SchemeVariable<bool>)operand2;

                switch (opType)
                {
                    case MethodOperatorType.FirstEqualSecond:
                        operand1.SetValue((bool)operand2.GetValue());
                        break;
                        //case OperatorType.FirstPlusSecond:
                        //    operand1.SetValue((bool)operand1.GetValue() + (bool)operand2.GetValue());
                        //    break;
                        //case OperatorType.FirstMinusSecond:
                        //    operand1.SetValue((string)operand1.GetValue() - (string)operand2.GetValue());
                        //    break;
                        //case OperatorType.FirstDividedBySecond:
                        //    operand1.SetValue((string)operand1.GetValue() / (string)operand2.GetValue());
                        //    break;
                        //case OperatorType.FirstMultypliedBySecond:
                        //    operand1.SetValue((string)operand1.GetValue() * (string)operand2.GetValue());
                        //    break;
                }
            }

            Variables[CheckVariableExistance(operand1Name)].SetValue(operand1);
        }
        #endregion

        #region Variable Comparsion
        public bool UseComparsion(string operand1Name, ConditionOperatorType opType, string operand2Name)
        {
            SchemeVariableBase operand1 = Variables[CheckVariableExistance(operand1Name)];
            SchemeVariableBase operand2 = Variables[CheckVariableExistance(operand2Name)];

            if ((operand1.ValueType != operand2.ValueType) || (operand1 == null || operand2 == null))
            {
                Debug.Log("Value types exception!");
                return false; // throw Exception();
            }

            if (operand1.ValueType == typeof(int) && operand2.ValueType == typeof(int))
            {
                switch (opType)
                {
                    case ConditionOperatorType.IsEqual:
                        if ((int)operand1.GetValue() == (int)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsNotEqual:
                        if ((int)operand1.GetValue() != (int)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsGreater:
                        if ((int)operand1.GetValue() > (int)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsGreaterOrEqual:
                        if ((int)operand1.GetValue() >= (int)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsLess:
                        if ((int)operand1.GetValue() < (int)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsLessOrEqual:
                        if ((int)operand1.GetValue() <= (int)operand2.GetValue()) return true;
                        break;
                }
            }

            else if (operand1.ValueType == typeof(float) && operand2.ValueType == typeof(float))
            {
                switch (opType)
                {
                    case ConditionOperatorType.IsEqual:
                        if ((float)operand1.GetValue() == (float)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsNotEqual:
                        if ((float)operand1.GetValue() != (float)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsGreater:
                        if ((float)operand1.GetValue() > (float)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsGreaterOrEqual:
                        if ((float)operand1.GetValue() >= (float)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsLess:
                        if ((float)operand1.GetValue() < (float)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsLessOrEqual:
                        if ((float)operand1.GetValue() <= (float)operand2.GetValue()) return true;
                        break;
                }
            }

            else if (operand1.ValueType == typeof(string) && operand2.ValueType == typeof(string))
            {
                string first;
                string second;

                switch (opType)
                {
                    case ConditionOperatorType.IsEqual:
                        if ((string)operand1.GetValue() == (string)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsNotEqual:
                        if ((string)operand1.GetValue() != (string)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsGreater:
                        first = (string)operand1.GetValue();
                        second = (string)operand2.GetValue();
                        if (first.Length > second.Length) return true;
                        break;
                    case ConditionOperatorType.IsGreaterOrEqual:
                        first = (string)operand1.GetValue();
                        second = (string)operand2.GetValue();
                        if (first.Length >= second.Length) return true;
                        break;
                    case ConditionOperatorType.IsLess:
                        first = (string)operand1.GetValue();
                        second = (string)operand2.GetValue();
                        if (first.Length < second.Length) return true;
                        break;
                    case ConditionOperatorType.IsLessOrEqual:
                        first = (string)operand1.GetValue();
                        second = (string)operand2.GetValue();
                        if (first.Length <= second.Length) return true;
                        break;
                }
            }

            else if (operand1.ValueType == typeof(bool) && operand2.ValueType == typeof(bool))
            {
                switch (opType)
                {
                    case ConditionOperatorType.IsEqual:
                        if ((bool)operand1.GetValue() == (bool)operand2.GetValue()) return true;
                        break;
                    case ConditionOperatorType.IsNotEqual:
                        if ((bool)operand1.GetValue() != (bool)operand2.GetValue()) return true; break;
                        //case ConditionalOperatorType.IsGreater:
                        //break;
                        //case ConditionalOperatorType.IsGreaterOrEqual:
                        //    break;
                        //case ConditionalOperatorType.IsLess:
                        //    break;
                        //case ConditionalOperatorType.IsLessOrEqual:
                        //    break;
                }
            }

            return false;
        }
    }
    #endregion
}
