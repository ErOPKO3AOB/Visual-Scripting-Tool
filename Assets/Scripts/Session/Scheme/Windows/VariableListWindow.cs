using GlobalServices.ProjectLifetime;
using Session.Scheme.Variables;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class VariableListWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowService, VariableService variableService)
        {
            _windowService = windowService;
            _variableService = variableService;
        }

        private WindowFactory _windowService;
        private VariableService _variableService;

        [Header("UI")]
        [SerializeField] private VariableItemUI _variableItemPrefab;
        [SerializeField] private Button _addNewVariableButton;
        [SerializeField] private LayoutElement _content;
        [SerializeField] private Button _closeButton;

        private List<VariableItemUI> _activeVariableItems = new List<VariableItemUI>();

        private VariablePickerUI _variablePicker;

        public override void SetSender(object sender)
        {
            try
            {
                _variablePicker = (VariablePickerUI)sender;
                RebuildUI();
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Start()
        {
            _addNewVariableButton.onClick.AddListener(() =>
            {
                VariableItemUI window = (VariableItemUI)_windowService.OpenWindow(_variableItemPrefab.WindowName, _content.transform);
                window.MasterList = this;
                _activeVariableItems.Add(window);
            });

            if (_windowService == null) Debug.Log("Window service is null");
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(WindowName); });
        }

        private void RebuildUI()
        {
            for (int i = 0; i < _variableService.Variables.Count; i++)
            {
                SchemeVariableBase schemeVariable = _variableService.Variables[i];

                VariableItemUI window = (VariableItemUI)_windowService.OpenWindow(_variableItemPrefab.WindowName, _content.transform);

                window.MasterList = this;
                window.RebuildUI(_variableService.GetTypeIntegerValue(schemeVariable.ValueType), schemeVariable.variableName, schemeVariable.GetValue());

                _activeVariableItems.Add(window);
            }
        }

        private void OnDestroy()
        {
            _addNewVariableButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }

        public void AddOrModifyVariable(string name, Type type, object value = null)
        {
            if (type != null && name != null)
            {
                switch (_variableService.GetTypeIntegerValue(type))
                {
                    case 0:
                        int intValue = 0;
                        if (value != null)
                            intValue = int.Parse(value.ToString());
                        _variableService.BuildVariable(name, intValue);
                        break;

                    case 1:
                        float floatValue = 0;
                        if (value != null)
                            floatValue = float.Parse(value.ToString());
                        _variableService.BuildVariable(name, floatValue);
                        break;

                    case 2:
                        string stringValue = "";
                        if (value != null)
                            stringValue = value.ToString();
                        _variableService.BuildVariable(name, stringValue);
                        break;

                    case 3:
                        bool boolValue = false;
                        if (value != null) bool.Parse(value.ToString());
                        else boolValue = false;
                        _variableService.BuildVariable(name, boolValue);
                        break;
                }
            }
        }

        public void RemoveVariable(VariableItemUI variableItem)
        {
            SchemeVariableBase variable = _variableService.Variables.Find(v => v.variableName == variableItem.VariableName);

            _activeVariableItems.Remove(variableItem);
            _variableService.RemoveVariable(variableItem.VariableName);
            Destroy(variableItem.gameObject);
        }

        public void ChooseVariable(string variableName)
        {
            int variableIndex = _variableService.CheckVariableExistance(variableName);
            Debug.Log($"Var index: {variableIndex} and var name: {variableName}");
            if (variableIndex > -1)
                _variablePicker.OnVariableChoose(_variableService.Variables[variableIndex]);

            _windowService.CloseWindow(WindowName);
        }
    }
}