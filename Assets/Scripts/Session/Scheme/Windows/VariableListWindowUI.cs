using GlobalServices.ProjectLifetime;
using Session.Scheme.Variables;
using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class VariableListWindowUI : BaseWindowUI
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

        private void Start()
        {
            for (int i = 0; i < _variableService.Variables.Count; i++)
            {
                SchemeVariableBase schemeVariable = _variableService.Variables[i];

                VariableItemUI window = (VariableItemUI)_windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[1].WindowName, _content.transform);

                window.MasterList = this;
                window.RebuildUI(GetTypeIntegerValue(schemeVariable.ValueType), schemeVariable.variableName, schemeVariable.GetValue());

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
            if (name != null && type != null)
            {
                switch (GetTypeIntegerValue(type))
                {
                    case 0:
                        _variableService.BuildVariable<int>(name, value);
                        break;
                    case 1:
                        _variableService.BuildVariable<float>(name, value);
                        break;
                    case 2:
                        _variableService.BuildVariable<string>(name, value);
                        break;
                    case 3:
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
            Debug.Log("REMOWE");

            Destroy(variable.gameObject);

            if (_variableService.Variables.Count >= _activeVariableItems.Count)
            {
                for (int i = 0; i < _activeVariableItems.Count; i++)
                {
                    if (_variableService.Variables[i].variableName == name)
                    {
                        _variableService.RemoveVariable(name);
                        _activeVariableItems.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private int GetTypeIntegerValue(Type type)
        {
            int typeValue = 0;

            switch (type)
            {
                case Type intType when intType == typeof(int):
                    typeValue = 0;
                    break;
                case Type floatType when floatType == typeof(float):
                    typeValue = 1;
                    break;
                case Type stringType when stringType == typeof(string):
                    typeValue = 2;
                    break;
                case Type boolType when boolType == typeof(bool):
                    typeValue = 3;
                    break;
            }

            return typeValue;
        }
    }
}