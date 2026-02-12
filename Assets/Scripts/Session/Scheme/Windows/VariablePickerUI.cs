using Session.Scheme.Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Variables
{
    public class VariablePickerUI : BaseWindow
    {
        [Header("Configs")]
        [SerializeField] private VariableListWindow _variableListPrefab;
        [SerializeField] private ChoosedVariableItem _choosedVariablePrefab;

        [Header("UI")]
        [SerializeField] private Transform _content;
        [SerializeField] private Button _addNewButton;

        private WindowFactory _windowService;
        public VariableListWindow VariableList { get; private set; }


        public UnityAction<SchemeVariableBase> OnVariableChoosed;
        public UnityAction<SchemeVariableBase> OnVariableDeleted;

        public ChoosedVariableItem VariableItem { get; private set; }

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

            VariableItem = variableItem;
            _addNewButton.gameObject.SetActive(false);

            OnVariableChoosed?.Invoke(variable);
        }


        public void OnVariableDelete()
        {
            OnVariableDeleted?.Invoke(VariableItem.SchemeVariable);
            Destroy(VariableItem.gameObject);
            VariableItem = null;

            _addNewButton.gameObject.SetActive(true);
        }
    }
}