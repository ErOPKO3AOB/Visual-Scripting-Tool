using System;
using System.Collections.Generic;
using UnityEngine;

namespace Session.Scheme.Variables
{
    public class VariableService
    {
        public List<SchemeVariableBase> Variables { get; private set; } = new List<SchemeVariableBase>();

        public void BuildVariable<T>(string varName, object startValue = null)
        {
            if (string.IsNullOrEmpty(varName)) return;

            int index = CheckExistance(varName);

            if (index < 0)
            {
                SchemeVariableBase schemeVariable = new SchemeVariable<T>(varName);
                Variables.Add(schemeVariable);
                schemeVariable.SetValue(startValue);
            }

            else
            {
                SetValueToVariable(varName, startValue);
                Debug.Log($"Setting value to variable: name {varName} value {startValue}");
            }
        }

        public void SetTypeToVariable<T>(string varName)
        {
            int index = CheckExistance(varName);

            if (index > -1)
            {
                SchemeVariableBase schemeVariable = Variables[index];
                RemoveVariable(schemeVariable.variableName);
                BuildVariable<T>(schemeVariable.variableName, schemeVariable.GetValue());
            }
        }

        public void SetValueToVariable(string varName, object value)
        {
            int index = CheckExistance(varName);

            if (index > -1)
            {
                Variables[index].SetValue(value);
            }
        }

        public void RemoveVariable(string varName)
        {
            int index = CheckExistance(varName);

            if (index > -1)
            {
                Variables.RemoveAt(index);
            }
        }

        public int CheckExistance(string varName)
        {
            if (Variables.Count > 0)
            {
                for (int i = 0; i < Variables.Count; i++)
                {
                    if (Variables[i].variableName == varName)
                    {
                        return i;
                    }
                }
            }

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
    }
}