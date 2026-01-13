using System.Collections.Generic;

namespace Session.Scheme.Variables
{
    public class VariableService
    {
        public List<SchemeVariableBase> Variables { get; private set; }

        public void BuildVariable<T>(string varName, object startValue = null)
        {
            int index = CheckExistance(varName);

            if (index < 0)
            {
                SchemeVariableBase schemeVariable = new SchemeVariable<T>(varName);
                schemeVariable.SetValue(startValue);
                Variables.Add(schemeVariable);
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

        private int CheckExistance(string varName)
        {
            for (int i = 0; i < Variables.Count; i++)
            {
                if (Variables[i].name == varName)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}