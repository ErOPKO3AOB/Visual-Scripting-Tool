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

        private readonly List<VariableItem> _activeVariableItems = new();

        private VariablePickerItem _variablePicker;

        public bool HasSender => _variablePicker != null;

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
                _windowService.OpenWindow(_variableItemPrefab, _content.transform, this);
            });

            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(this); });
        }

        private void RebuildUI()
        {
            if (_activeVariableItems.Count > 0)
            {
                _activeVariableItems.ForEach(variable => Destroy(variable.gameObject));
                _activeVariableItems.Clear();
            }

            for (int i = 0; i < _variableService.Variables.Count; i++)
            {
                VariableItem window = (VariableItem)_windowService.OpenWindow(_variableItemPrefab, _content.transform, this);
                window.SchemeVariable = _variableService.Variables[i];
            }
        }

        private void OnDestroy()
        {
            _addNewVariableButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }

        public void AddOrModifyVariable(VariableItem item)
        {
            //if (item == null)
            //    Debug.Log("item == null");
            //if (item.SchemeVariable == null)
            //    Debug.Log("item.SchemeVariable == null");
            //if (item.SchemeVariable.ValueType == null)
            //    Debug.Log("item.SchemeVariable.ValueType == null");
            //if (item.SchemeVariable.GetStartValue() == null)
            //    Debug.Log("item.SchemeVariable.GetValue() == null");
            //if (item.SchemeVariable.variableName == null)
            //    Debug.Log("item.SchemeVariable.variableName == null");

            if (item.SchemeVariable.ValueType == typeof(int))
                _variableService.BuildVariable<int>(item.SchemeVariable.variableName, int.Parse(item.SchemeVariable.GetStartValue().ToString()));
            else if (item.SchemeVariable.ValueType == typeof(float))
                _variableService.BuildVariable<float>(item.SchemeVariable.variableName, float.Parse(item.SchemeVariable.GetStartValue().ToString()));
            else if (item.SchemeVariable.ValueType == typeof(string))
                _variableService.BuildVariable<string>(item.SchemeVariable.variableName, item.SchemeVariable.GetStartValue().ToString());
            else if (item.SchemeVariable.ValueType == typeof(bool))
                _variableService.BuildVariable<bool>(item.SchemeVariable.variableName, bool.Parse(item.SchemeVariable.GetStartValue().ToString()));

            _activeVariableItems.Add(item);
        }

        public void RemoveVariable(VariableItem variableItem)
        {
            SchemeVariableBase variable = _variableService.Variables.Find(v => v.variableName == variableItem.SchemeVariable.variableName);

            if (variableItem.SchemeVariable != null && _variableService.CheckVariableExistance(variableItem.SchemeVariable.variableName) > -1)
                _variableService.RemoveVariable(variableItem.SchemeVariable.variableName);

            if (_activeVariableItems.Contains(variableItem))
                _activeVariableItems.Remove(variableItem);

            Destroy(variableItem.gameObject);
        }

        public void ChooseVariable(string variableName)
        {
            if (_variablePicker == null) return;

            int variableIndex = _variableService.CheckVariableExistance(variableName);
            Debug.Log($"Choosed var: index {variableIndex} and var name: {variableName}");
            if (variableIndex > -1)
                _variablePicker.ChooseVariable(_variableService.Variables[variableIndex]);

            _windowService.CloseWindow(this);
        }
    }
}