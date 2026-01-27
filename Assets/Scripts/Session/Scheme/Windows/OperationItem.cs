using Session.Scheme.Variables;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Session.Scheme.Windows
{
    public class OperationItem : BaseWindowUI
    {
        [Header("UI")]
        [SerializeField] private TMP_Dropdown _operationDropDown;

        public UnityAction<VariableService.OperatorType> OnOperationTypeChoosed;

        private void Start()
        {
            InitializeDropdown();

            _operationDropDown.onValueChanged.AddListener((int value) =>
            {
                VariableService.OperatorType operatorType = (VariableService.OperatorType)value;

                OnOperationTypeChoosed?.Invoke(operatorType);
            });
        }

        private void InitializeDropdown()
        {
            _operationDropDown.ClearOptions();

            var enumValues = Enum.GetValues(typeof(VariableService.OperatorType));
            var enumType = typeof(VariableService.OperatorType);

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