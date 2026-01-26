using Session.Scheme.Variables;
using UnityEngine;

namespace Session.Scheme.Operators
{
    public class VariableOperatorService
    {
        public enum OperatorType
        {
            FirstEqualSecond, FirstPlusSecond, FirstMinusSecond, FirstDividedBySecond, FirstMultypliedBySecond
        }

        public void UseOperation(SchemeVariableBase operand1, OperatorType opType, SchemeVariableBase operand2)
        {
            if ((operand1.ValueType != operand2.ValueType))
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
                    case OperatorType.FirstEqualSecond:
                        operand1.SetValue((int)operand2.GetValue());
                        break;
                    case OperatorType.FirstPlusSecond:
                        operand1.SetValue((int)operand1.GetValue() + (int)operand2.GetValue());
                        break;
                    case OperatorType.FirstMinusSecond:
                        operand1.SetValue((int)operand1.GetValue() - (int)operand2.GetValue());
                        break;
                    case OperatorType.FirstDividedBySecond:
                        operand1.SetValue((int)operand1.GetValue() / (int)operand2.GetValue());
                        break;
                    case OperatorType.FirstMultypliedBySecond:
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
                    case OperatorType.FirstEqualSecond:
                        operand1.SetValue((float)operand2.GetValue());
                        break;
                    case OperatorType.FirstPlusSecond:
                        operand1.SetValue((float)operand1.GetValue() + (float)operand2.GetValue());
                        break;
                    case OperatorType.FirstMinusSecond:
                        operand1.SetValue((float)operand1.GetValue() - (float)operand2.GetValue());
                        break;
                    case OperatorType.FirstDividedBySecond:
                        operand1.SetValue((float)operand1.GetValue() / (float)operand2.GetValue());
                        break;
                    case OperatorType.FirstMultypliedBySecond:
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
                    case OperatorType.FirstEqualSecond:
                        operand1.SetValue((string)operand2.GetValue());
                        break;
                    case OperatorType.FirstPlusSecond:
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
                    case OperatorType.FirstEqualSecond:
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
        }
    }
}