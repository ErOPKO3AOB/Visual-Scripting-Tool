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
        public void Construct(BlockConfigs blockConfigs, WindowFactory windowService, VariableService variableService)
        {
            _blockConfigs = blockConfigs;
            _windowService = windowService;
            _variableService = variableService;
        }

        private BlockConfigs _blockConfigs;
        private WindowFactory _windowService;
        private VariableService _variableService;

        [Header("UI")]
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
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Start()
        {
            RebuildUI();

            _addNewVariableButton.onClick.AddListener(() =>
            {
                VariableItemUI window = (VariableItemUI)_windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[11].WindowName, _content.transform);
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

                VariableItemUI window = (VariableItemUI)_windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[11].WindowName, _content.transform);

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

        public void AddVariable(string name, Type type, object value)
        {
            if (name != null && type != null)
            {
                switch (_variableService.GetTypeIntegerValue(type))
                {
                    case 0:
                        if (value != null) int.Parse(value.ToString());
                        else value = 0;
                        _variableService.BuildVariable<int>(name, value);
                        break;
                    case 1:
                        if (value != null) float.Parse(value.ToString());
                        else value = 0;
                        _variableService.BuildVariable<float>(name, value);
                        break;
                    case 2:
                        if (value != null) value.ToString();
                        else value = "";
                        _variableService.BuildVariable<string>(name, value);
                        break;
                    case 3:
                        if (value != null) bool.Parse(value.ToString());
                        else value = false;
                        _variableService.BuildVariable<bool>(name, value);
                        break;
                }
            }

            else
            {
                Debug.LogWarning("Не указан один из важных параметров для создания переменной!");
            }
        }

        public void RemoveVariable(VariableItemUI variableItem)
        {
            SchemeVariableBase variable = _variableService.Variables.Find(v => v.variableName == variableItem.VariableName);
            _variableService.RemoveVariable(variableItem.VariableName);

            _activeVariableItems.Remove(variableItem);
            Destroy(variableItem.gameObject);
        }

        public void ChooseVariable(string variableName)
        {
            int variableIndex = _variableService.CheckExistance(_activeVariableItems.Find(v => v.VariableName == variableName).VariableName);
            if (variableIndex != -1)
                _variablePicker.OnVariableChoose(_variableService.Variables[variableIndex]);
        }
    }
}