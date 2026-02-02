using Session.Scheme.Windows;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Variables
{
    public class VariablePickerUI : BaseWindow
    {
        public enum VariablePickType
        {
            Single, Multiple
        }

        [Header("Configs")]
        [SerializeField] private VariableListWindow _variableListPrefab;
        [SerializeField] private VariablePickType _pickType;
        public VariablePickType PickType { get { return _pickType; } set { _pickType = value; } }
        [SerializeField] private ChoosedVariableItem _choosedVariablePrefab;

        [Header("UI")]
        [SerializeField] private Transform _content;
        [SerializeField] private Button _addNewButton;

        private WindowService _windowService;
        public VariableListWindow VariableList { get; private set; }

        public List<ChoosedVariableItem> VariableItems { get; private set; } = new List<ChoosedVariableItem>(1);

        public UnityAction<SchemeVariableBase> OnVariableChoosed;
        public UnityAction<SchemeVariableBase> OnVariableDeleted;

        [Inject]
        public void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        private void Start()
        {
            _addNewButton.onClick.AddListener(() =>
            {
                VariableList = (VariableListWindow)_windowService.OpenWindow(_variableListPrefab.WindowName, sender: this);
            });

            //_windowService.OnCloseWindow += (BaseWindow window) =>
            //{
            //    if (window.GetType() == typeof(VariableListWindow))
            //    {
            //        VariableList.OnVariableChoose -= OnVariableChoose;
            //    }
            //};
        }

        public void OnVariableChoose(SchemeVariableBase variable)
        {
            _addNewButton.transform.parent = null;
            ChoosedVariableItem variableItem = Instantiate(_choosedVariablePrefab, _content.transform).GetComponent<ChoosedVariableItem>();
            VariableItems.Add(variableItem);
            variableItem.Initialize(this, variable);
            _addNewButton.transform.SetParent(_content.transform);

            OnVariableChoosed?.Invoke(variable);
        }

        public void OnVariableDelete(ChoosedVariableItem variableItem)
        {
            OnVariableDeleted?.Invoke(variableItem.SchemeVariable);
            
            VariableItems.Remove(variableItem);
            Destroy(variableItem.gameObject);
        }
    }
}