using System;

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

        public override void SetValue(object value)
        {
            if (value is T typedValue)
                Value = typedValue;
            else
                throw new InvalidCastException($"Cannot cast {Value?.GetType()} to {typeof(T)}");
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}