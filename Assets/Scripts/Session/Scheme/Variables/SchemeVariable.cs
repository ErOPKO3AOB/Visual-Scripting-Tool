using System;
using UnityEngine;

namespace Session.Scheme.Variables
{

    [Serializable]
    public sealed class SchemeVariable<T> : SchemeVariableBase
    {
        public T Value { get; private set; }

        public SchemeVariable(string name) : base(name)
        {

        }

        public override Type ValueType => typeof(T);

        public override void SetValue(object value) // Пофиксить проблему с разными типами
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                Value = default;
                Debug.Log($"value {value} is defaul");
            }

            else
            {
                Debug.Log($"{Value.GetType()} + {value.GetType()}");

                if (Value.GetType() == value.GetType())
                    Value = (T)value;
            }
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}