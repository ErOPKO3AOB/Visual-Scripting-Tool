using Session.Scheme.Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Variables
{
    public class VariablePickerItem : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowService, VariableService variableService)
        {
            _windowService = windowService;
            _variableService = variableService;
        }

        private WindowFactory _windowService;
        private VariableService _variableService;


        [Header("Configs")]
        [SerializeField] private VariableListWindow _variableListPrefab;
        [SerializeField] private ChoosedVariableItem _choosedVariablePrefab;

        [Header("UI")]
        [SerializeField] private Transform _content;
        [SerializeField] private Button _addNewButton;


        public UnityAction<SchemeVariableBase> OnVariableChanged;

        public ChoosedVariableItem ChoosedVariableItem { get; private set; }


        private void Start()
        {
            _addNewButton.onClick.AddListener(() =>
            {
                _windowService.OpenWindow(_variableListPrefab, sender: this);
            });
        }

        public void ChooseVariable(SchemeVariableBase variable)
        {
            if (variable == null)
            {
                DeleteVariable();
            }

            else
            {
                _addNewButton.transform.SetParent(null);
                ChoosedVariableItem variableItem = Instantiate(_choosedVariablePrefab, _content.transform).GetComponent<ChoosedVariableItem>();
                variableItem.Initialize(this, variable);
                _addNewButton.transform.SetParent(_content.transform);

                ChoosedVariableItem = variableItem;
                _addNewButton.gameObject.SetActive(false);

                OnVariableChanged?.Invoke(variable);
            }
        }

        public void DeleteVariable()
        {
            if (ChoosedVariableItem != null)
            {
                Destroy(ChoosedVariableItem.gameObject);
                ChoosedVariableItem = null;

                _addNewButton.gameObject.SetActive(true);
                OnVariableChanged?.Invoke(null);
            }
        }

        private void OnDestroy()
        {
            _addNewButton.onClick.RemoveAllListeners();
        }
    }
}