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

        public UnityAction<SchemeVariableBase> OnVariableChoose;
        public UnityAction<SchemeVariableBase> OnVariableDelete;

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
                _windowService.OpenWindow(_variableListPrefab, sender: this);
            });
        }

        public void ChooseVariable(SchemeVariableBase variable)
        {
            _addNewButton.transform.SetParent(null);
            ChoosedVariableItem variableItem = Instantiate(_choosedVariablePrefab, _content.transform).GetComponent<ChoosedVariableItem>();
            variableItem.Initialize(this, variable);
            _addNewButton.transform.SetParent(_content.transform);

            VariableItem = variableItem;
            _addNewButton.gameObject.SetActive(false);

            OnVariableChoose?.Invoke(variable);
        }

        public void DeleteVariable()
        {
            OnVariableDelete?.Invoke(VariableItem.SchemeVariable);
            Destroy(VariableItem.gameObject);
            VariableItem = null;

            _addNewButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _addNewButton.onClick.RemoveAllListeners();
        }
    }
}