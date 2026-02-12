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
                        Value = (T)(object)Convert.ToInt32(value);
                    if (typeof(T) == typeof(float))
                        Value = (T)(object)Convert.ToSingle(value);
                    if (typeof(T) == typeof(bool))
                        Value = (T)(object)Convert.ToBoolean(value);
                    if (typeof(T) == typeof(string))
                        Value = (T)(object)Convert.ToString(value);
                    else
                        Value = (T)Convert.ChangeType(value, typeof(T));
                }

                catch (Exception e)
                {
                    Debug.LogError($"Error setting value: attempted to assign {value} ({value?.GetType()}), but failed. Exception: {e}");
                }
            }
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}