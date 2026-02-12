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
        [SerializeField] private ChoosedVariableItem _choosedVariablePrefab;

        [Header("UI")]
        [SerializeField] private Transform _content;
        [SerializeField] private Button _addNewButton;

        private WindowFactory _windowService;
        public VariableListWindow VariableList { get; private set; }


        public UnityAction<SchemeVariableBase> OnVariableChoosed;
        public UnityAction<SchemeVariableBase> OnVariableDeleted;

        public List<ChoosedVariableItem> VariableItems { get; private set; } = new List<ChoosedVariableItem>();
        public VariablePickType PickType { get { return _pickType; } set { _pickType = value; } }

        [Inject]
        public void Construct(WindowFactory windowService)
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
            _addNewButton.transform.SetParent(null);
            ChoosedVariableItem variableItem = Instantiate(_choosedVariablePrefab, _content.transform).GetComponent<ChoosedVariableItem>();
            variableItem.Initialize(this, variable);
            _addNewButton.transform.SetParent(_content.transform);

            if (!VariableItems.Contains(variableItem))
            {
                if ((_pickType == VariablePickType.Single && VariableItems.Count == 0)
                    || _pickType == VariablePickType.Multiple)
                {
                    VariableItems.Add(variableItem);
                    OnVariableChoosed?.Invoke(variable);
                    
                    switch (_pickType)
                    {
                        case VariablePickType.Single:
                            _addNewButton.gameObject.SetActive(false);
                            break;
                        case VariablePickType.Multiple:
                            _addNewButton.gameObject.SetActive(true);
                            break;
                    }
                }
            }

        }

        public void OnVariableDelete(ChoosedVariableItem variableItem)
        {
            OnVariableDeleted?.Invoke(variableItem.SchemeVariable);

            VariableItems.Remove(variableItem);
            Destroy(variableItem.gameObject);

            _addNewButton.gameObject.SetActive(true);
        }
    }
}