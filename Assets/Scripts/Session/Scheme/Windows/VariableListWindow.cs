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
        [SerializeField] private VariableItem _variableItemPrefab;
        [SerializeField] private Button _addNewVariableButton;
        [SerializeField] private LayoutElement _content;
        [SerializeField] private Button _closeButton;

        private List<VariableItem> _activeVariableItems = new List<VariableItem>();

        private VariablePickerItem _variablePicker;

        public override void SetSender(object sender)
        {
            try
            {
                if (sender is VariablePickerItem variablePicker)
                    _variablePicker = variablePicker;
                else
                    _variablePicker = null;

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
                VariableItem window = (VariableItem)_windowService.OpenWindow(_variableItemPrefab, _content.transform, this);
                _activeVariableItems.Add(window);
            });

            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(this); });
        }

        private void RebuildUI()
        {
            for (int i = 0; i < _variableService.Variables.Count; i++)
            {
                SchemeVariableBase schemeVariable = _variableService.Variables[i];

                VariableItem window = (VariableItem)_windowService.OpenWindow(_variableItemPrefab, _content.transform);

                _activeVariableItems.Add(window);
            }
        }

        private void OnDestroy()
        {
            _addNewVariableButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }

        public void AddOrModifyVariable(SchemeVariableBase schemeVariable)
        {
            if (schemeVariable != null)
            {
                if (schemeVariable.GetValue().GetType() == typeof(int))
                    _variableService.BuildVariable<int>(schemeVariable.variableName, int.Parse(schemeVariable.GetValue().ToString()));
                else if (schemeVariable.GetValue().GetType() == typeof(float))
                    _variableService.BuildVariable<float>(schemeVariable.variableName, float.Parse(schemeVariable.GetValue().ToString()));
                else if (schemeVariable.GetValue().GetType() == typeof(string))
                    _variableService.BuildVariable<string>(schemeVariable.variableName, schemeVariable.GetValue().ToString());
                else if (schemeVariable.GetValue().GetType() == typeof(bool))
                        _variableService.BuildVariable<bool>(schemeVariable.variableName, bool.Parse(schemeVariable.GetValue().ToString()));
            }
        }

        public void RemoveVariable(VariableItem variableItem)
        {
            SchemeVariableBase variable = _variableService.Variables.Find(v => v.variableName == variableItem.SchemeVariable.variableName);

            _activeVariableItems.Remove(variableItem);
            _variableService.RemoveVariable(variableItem.SchemeVariable.variableName);
            Destroy(variableItem.gameObject);
        }

        public void ChooseVariable(string variableName)
        {
            if (_variablePicker == null) return;

            int variableIndex = _variableService.CheckVariableExistance(variableName);
            //Debug.Log($"Var index: {variableIndex} and var name: {variableName}");
            if (variableIndex > -1)
                _variablePicker.ChooseVariable(_variableService.Variables[variableIndex]);

            _windowService.CloseWindow(this);
        }
    }
}