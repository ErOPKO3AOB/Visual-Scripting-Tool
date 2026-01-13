using System;

namespace Session.Scheme.Variables
{
    [Serializable]
    public abstract class SchemeVariableBase
    {
        public readonly string name;

        public SchemeVariableBase(string name)
        {
            this.name = name;
        }

        public abstract Type ValueType { get; }
        public abstract void SetValue(object typedValue);
    }
}