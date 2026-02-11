using Session.Scheme.Variables;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Session.Scheme.Windows
{
    public class OperationItem : BaseWindow
    {
        public enum OperationType
        {
            Method, Condition
        }

        [Header("UI")]
        [SerializeField] private TMP_Dropdown _operationDropDown;
        public TMP_Dropdown OperationDropDown => _operationDropDown;

        private OperationType _operatorType;
        public UnityAction<object> OnOperationTypeChoosed;
        
        public OperationType OperatorType 
        {
            get 
            {
                return _operatorType; 
            }
            
            set 
            {
                _operatorType = value;
                
                InitializeDropdown();

                _operationDropDown.onValueChanged.AddListener((int value) =>
                {
                    Type operatorType = default;

                    switch (_operatorType)
                    {
                        case OperationType.Method:
                            operatorType = typeof(VariableService.MethodOperatorType);
                            break;
                        case OperationType.Condition:
                            operatorType = typeof(VariableService.ConditionOperatorType);
                            break;
                    }

                    OnOperationTypeChoosed?.Invoke(value);
                });
            } 
        }

        private void InitializeDropdown()
        {
            _operationDropDown.ClearOptions();

            Type enumType = default;

            switch (_operatorType)
            {
                case OperationType.Method:
                    enumType = typeof(VariableService.MethodOperatorType);
                    break;
                case OperationType.Condition:
                    enumType = typeof(VariableService.ConditionOperatorType);
                    break;
            }

            var enumValues = Enum.GetValues(enumType);
            var options = new System.Collections.Generic.List<TMP_Dropdown.OptionData>();

            foreach (var value in enumValues)
            {
                var field = enumType.GetField(value.ToString());
                var attribute = (InspectorNameAttribute)Attribute.GetCustomAttribute(
                    field, typeof(InspectorNameAttribute));

                string displayName = attribute != null ? attribute.displayName : value.ToString();
                options.Add(new TMP_Dropdown.OptionData(displayName));
            }

            _operationDropDown.AddOptions(options);
        }

        private void OnDestroy()
        {
            _operationDropDown.onValueChanged.RemoveAllListeners();
        }
    }
}