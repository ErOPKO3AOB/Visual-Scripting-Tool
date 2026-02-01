using GlobalServices.ProjectLifetime;
using Session.Scheme.Variables;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class VariableListWindowUI : BaseWindow
    {
        [Inject]
        public void Construct(BlockConfigs blockConfigs, WindowService windowService, VariableService variableService)
        {
            _blockConfigs = blockConfigs;
            _windowService = windowService;
            _variableService = variableService;
        }

        private BlockConfigs _blockConfigs;
        private WindowService _windowService;
        private VariableService _variableService;

        [Header("UI")]
        [SerializeField] private Button _addNewVariableButton;
        [SerializeField] private LayoutElement _content;
        [SerializeField] private Button _closeButton;

        private List<VariableItemUI> _activeVariableItems = new List<VariableItemUI>();

        public UnityAction<SchemeVariableBase> OnVariableChoose;
        public UnityAction<SchemeVariableBase> OnVariableDelete;

        private void Start()
        {
            for (int i = 0; i < _variableService.Variables.Count; i++)
            {
                SchemeVariableBase schemeVariable = _variableService.Variables[i];

                VariableItemUI window = (VariableItemUI)_windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[1].WindowName, _content.transform);

                window.MasterList = this;
                window.RebuildUI(_variableService.GetTypeIntegerValue(schemeVariable.ValueType), schemeVariable.variableName, schemeVariable.GetValue());

                _activeVariableItems.Add(window);
            }

            _addNewVariableButton.onClick.AddListener(() =>
            {
                VariableItemUI window = (VariableItemUI)_windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[1].WindowName, _content.transform);
                window.MasterList = this;
                _activeVariableItems.Add(window);
            });

            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(WindowName); });
        }

        private void OnDestroy()
        {
            _addNewVariableButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }

        public void AddVariable(string name, Type type, object value)
        {
            int existingIndex = _variableService.CheckExistance(name);

            if (name != null && type != null)
            {
                switch (_variableService.GetTypeIntegerValue(type))
                {
                    case 0:
                        if (value != null) int.Parse(value.ToString());
                        _variableService.BuildVariable<int>(name, value);
                        break;
                    case 1:
                        if (value != null) float.Parse(value.ToString());
                        _variableService.BuildVariable<float>(name, value);
                        break;
                    case 2:
                        if (value != null) value.ToString();
                        _variableService.BuildVariable<string>(name, value);
                        break;
                    case 3:
                        if (value != null) bool.Parse(value.ToString());
                        _variableService.BuildVariable<bool>(name, value);
                        break;
                }
            }

            else
            {
                Debug.LogWarning("Не указан один из важных параметров для создания переменной!");
            }
        }

        public void RemoveVariable(VariableItemUI variable)
        {
            Destroy(variable.gameObject);

            if (_variableService.Variables.Count >= _activeVariableItems.Count)
            {
                for (int i = 0; i < _activeVariableItems.Count; i++)
                {
                    if (_variableService.Variables[i].variableName == variable.name)
                    {
                        OnVariableDelete?.Invoke(_variableService.Variables[i]);
                        _variableService.RemoveVariable(name);
                        _activeVariableItems.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void ChooseVariable(string variableName)
        {
            Debug.Log($"CHOOSING VARIABLE {variableName}");
            for (int i = 0; i < _activeVariableItems.Count; i++)
            {
                if (_variableService.Variables[i].variableName == variableName)
                {
                    OnVariableChoose?.Invoke(_variableService.Variables[i]);
                    break;
                }
            }
        }
    }
}