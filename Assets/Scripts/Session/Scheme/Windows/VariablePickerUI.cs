using Session.Scheme.Windows;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Variables
{
    public class VariablePickerUI : BaseWindowUI
    {
        public enum VariablePickType
        {
            Single, Multiple
        }

        [Header("Configs")]
        [SerializeField] private VariableListWindowUI _variableListPrefab;
        [SerializeField] private VariablePickType _pickType;
        public VariablePickType PickType { get { return _pickType; } set { _pickType = value; } }
        [SerializeField] private ChoosedVariableItem _choosedVariablePrefab;

        [Header("UI")]
        [SerializeField] private Transform _content;
        [SerializeField] private Button _addNewButton;

        private WindowService _windowService;
        public VariableListWindowUI VariableList { get; private set; }

        public List<ChoosedVariableItem> VariableItems { get; private set; } = new List<ChoosedVariableItem>(1);

        [Inject]
        public void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        private void Start()
        {
            _addNewButton.onClick.AddListener(() =>
            {
                VariableList = (VariableListWindowUI)_windowService.OpenWindow(_variableListPrefab.WindowName);
                VariableList.OnVariableChoose += OnVariableChoose;
            });

            _windowService.OnCloseWindow += (BaseWindowUI window) =>
            {
                if (window.GetType() == typeof(VariableListWindowUI))
                {
                    VariableList.OnVariableChoose -= OnVariableChoose;
                }
            };
        }

        private void OnVariableChoose(SchemeVariableBase variable)
        {
            _addNewButton.transform.parent = null;
            ChoosedVariableItem variableItem = Instantiate(_choosedVariablePrefab, _content.transform).GetComponent<ChoosedVariableItem>();
            VariableItems.Add(variableItem);
            variableItem.Initialize(this, variable);
            _addNewButton.transform.SetParent(_content.transform);
        }

        public void OnVariableDelete(ChoosedVariableItem variableItem)
        {
            VariableItems.Remove(variableItem);
            Destroy(variableItem.gameObject);
        }
    }
}