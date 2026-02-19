using System;

namespace Session.Scheme.Variables
{
    [Serializable]
    public abstract class SchemeVariableBase
    {
        public readonly string variableName;

        public SchemeVariableBase(string name)
        {
            this.variableName = name;
        }

        public abstract Type ValueType { get; }
        public abstract void SetValue(object value);
        public abstract void SetStartValue(object value);
        public abstract object GetValue();
        public abstract object GetStartValue();
    }
}