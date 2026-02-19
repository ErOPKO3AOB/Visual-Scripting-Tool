using System;
using UnityEngine;

namespace Session.Scheme.Variables
{

    [Serializable]
    public sealed class SchemeVariable<T> : SchemeVariableBase
    {
        public T Value { get; private set; }
        public T StartValue { get; private set; }

        public SchemeVariable(string name) : base(name) { }

        public override Type ValueType => typeof(T);

        public override void SetValue(object value)
        {
            ValueSetProcess(value);
            Debug.Log("VALUE SETTING");
        }

        public override void SetStartValue(object value)
        {
            ValueSetProcess(value, true);
            ValueSetProcess(value);

            Debug.Log("START VALUE SETTING");
        }

        private void ValueSetProcess(object value, bool startValue = false)
        {
            if (value == null) return;

            try
            {
                T buildedValue;
                if (typeof(T) == typeof(int))
                    buildedValue = (T)(object)int.Parse(value.ToString());
                if (typeof(T) == typeof(float))
                    buildedValue = (T)(object)float.Parse(value.ToString());
                if (typeof(T) == typeof(bool))
                    buildedValue = (T)(object)bool.Parse(value.ToString());
                if (typeof(T) == typeof(string))
                    buildedValue = (T)(object)value.ToString();
                // For absolute strange exceptions
                else
                    buildedValue = (T)Convert.ChangeType(value, typeof(T));

                if (startValue)
                    StartValue = buildedValue;
                else
                    Value = buildedValue;
            }

            catch (Exception e)
            {
                Debug.LogError($"Error setting value: attempted to assign {value} to variable with name ''{variableName}'' ({value?.GetType()}), but failed. Exception: {e}");
            }
        }

        public override object GetValue()
        {
            return Value;
        }

        public override object GetStartValue()
        {
            return StartValue;
        }
    }
}