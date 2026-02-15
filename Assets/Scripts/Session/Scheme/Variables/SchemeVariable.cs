using System;
using UnityEngine;

namespace Session.Scheme.Variables
{

    [Serializable]
    public sealed class SchemeVariable<T> : SchemeVariableBase
    {
        public T Value { get; private set; }

        public SchemeVariable(string name) : base(name) { }

        public override Type ValueType => typeof(T);

        public override void SetValue(object value)
        {
            if (value == null)
            {
                Value = default;
            }

            else
            {
                try
                {
                    if (typeof(T) == typeof(int))
                        Value = (T)(object)int.Parse(value.ToString());
                    if (typeof(T) == typeof(float))
                        Value = (T)(object)float.Parse(value.ToString());
                    if (typeof(T) == typeof(bool))
                        Value = (T)(object)bool.Parse(value.ToString());
                    if (typeof(T) == typeof(string))
                        Value = (T)(object)value.ToString();
                    // For absolute strange exceptions
                    else
                        Value = (T)Convert.ChangeType(value, typeof(T));
                }

                catch (Exception e)
                {
                    Debug.LogError($"Error setting value: attempted to assign {value} to variable with name ''{variableName}'' ({value?.GetType()}), but failed. Exception: {e}");
                }
            }
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}