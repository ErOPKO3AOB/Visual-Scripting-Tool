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
    public class VariableListWindowUI : BaseWindowUI
    {
        [Inject]
        public void Construct(BlockConfigs blockConfigs, WindowService windowService, VariableService variableService)
        {
            _blockConfigs = blockConfigs;
            _windowService = windowService;
            VariableService = variableService;
        }

        private BlockConfigs _blockConfigs;
        private WindowService _windowService;
        public VariableService VariableService { get; private set; }

        [Header("UI")]
        [SerializeField] private Button _addNewVariableButton;
        [SerializeField] private LayoutElement _content;
        [SerializeField] private Button _closeButton;

        private List<VariableItemUI> _activeVariableItems = new List<VariableItemUI>();

        public UnityAction<SchemeVariableBase> OnVariableChoose;
        public UnityAction<SchemeVariableBase> OnVariableDelete;

        private void Start()
        {
            for (int i = 0; i < VariableService.Variables.Count; i++)
            {
                SchemeVariableBase schemeVariable = VariableService.Variables[i];

                VariableItemUI window = (VariableItemUI)_windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[1].WindowName, _content.transform);

                window.MasterList = this;
                window.RebuildUI(VariableService.GetTypeIntegerValue(schemeVariable.ValueType), schemeVariable.variableName, schemeVariable.GetValue());

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
            int existingIndex = VariableService.CheckExistance(name);

            if (name != null && type != null)
            {
                switch (VariableService.GetTypeIntegerValue(type))
                {
                    case 0:
                        if (value != null) int.Parse(value.ToString());
                        VariableService.BuildVariable<int>(name, value);
                        break;
                    case 1:
                        if (value != null) float.Parse(value.ToString());
                        VariableService.BuildVariable<float>(name, value);
                        break;
                    case 2:
                        if (value != null) value.ToString();
                        VariableService.BuildVariable<string>(name, value);
                        break;
                    case 3:
                        if (value != null) bool.Parse(value.ToString());
                        VariableService.BuildVariable<bool>(name, value);
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

            if (VariableService.Variables.Count >= _activeVariableItems.Count)
            {
                for (int i = 0; i < _activeVariableItems.Count; i++)
                {
                    if (VariableService.Variables[i].variableName == variable.name)
                    {
                        OnVariableDelete?.Invoke(VariableService.Variables[i]);
                        VariableService.RemoveVariable(name);
                        _activeVariableItems.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void ChooseVariable(string variableName)
        {
            for (int i = 0; i < _activeVariableItems.Count; i++)
            {
                if (VariableService.Variables[i].variableName == variableName)
                {
                    Debug.Log($"CHOOSING VARIABLE {VariableService.Variables[i].ValueType} {VariableService.Variables[i].variableName}");

                    OnVariableChoose?.Invoke(VariableService.Variables[i]);
                    break;
                }
            }
        }
    }
}