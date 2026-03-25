using Extensions;
using Session.Scheme.Variables;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class VariableItem : BaseWindow
    {
        [Inject]
        public void Construct(VariableService variableService)
        {
            _variableService = variableService;
        }

        [Header("UI")]
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_Dropdown _typeDropdown;
        [SerializeField] private TMP_InputField _valueInputField;
        [SerializeField] private TMP_Text _valueInputFieldPlaceHolder;
        [SerializeField] private Button _chooseButton;
        [SerializeField] private Button _deleteButton;

        [Header("Value input validation")]
        [Header("Int")]
        [SerializeField] private string _intPlaceHolder = "¬ведите целое число";
        [Header("Float")]
        [SerializeField] private string _floatPlaceHolder = "¬ведите дробное число";
        [Header("String")]
        [SerializeField] private string _stringPlaceHolder = "¬ведите строку";
        [Header("Bool")]
        [SerializeField] private string _boolPlaceHolder = "¬ведите булевое значение true/false";
        [SerializeField] private BoolValidator _boolValidator;

        private VariableService _variableService;

        private Type _variableType = typeof(int);
        private string _variableName;
        private object _variableValue;

        private VariableListWindow _variableList;
        private SchemeVariableBase _schemeVariable;

        public SchemeVariableBase SchemeVariable { get => _schemeVariable; set => _schemeVariable = value; }

        public override void SetSender(object sender)
        {
            try
            {
                _variableList = (VariableListWindow)sender;
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        #region Initiallization
        private void Start()
        {
            RebuildUI();

            _nameInputField.onEndEdit.AddListener((string text) =>
            {
                _variableName = text;
                OnEndEdit();

                if (!string.IsNullOrEmpty(text))
                {
                    Invoke(nameof(DisableNameInput), 0f);
                }
            });

            _typeDropdown.onValueChanged.AddListener((int value) => { InitializeValuePlaceHolder(value); OnEndEdit(); });
            _valueInputField.onEndEdit.AddListener((string value) => { _variableValue = value; OnEndEdit(); });

            _chooseButton.onClick.AddListener(() =>
            {
                OnEndEdit();

                if (_schemeVariable != null)
                    _variableList.ChooseVariable(_schemeVariable.variableName);
            });

            _deleteButton.onClick.AddListener(() =>
            {
                _variableList.RemoveVariable(this);
            });
        }

        private void DisableNameInput()
        {
            _nameInputField.interactable = false;
        }

        private void RebuildUI()
        {
            InitializeValuePlaceHolder(0);

            if (_schemeVariable != null)
            {
                if (_schemeVariable.ValueType != null)
                {
                    int value = _variableService.GetTypeIntegerValue(_schemeVariable.ValueType);
                    _typeDropdown.value = value;
                    InitializeValuePlaceHolder(value);
                }

                if (_schemeVariable.variableName != null)
                {
                    _nameInputField.SetTextWithoutNotify(_schemeVariable.variableName);
                    _nameInputField.interactable = false;
                }
                if (_schemeVariable.GetStartValue() != null)
                    _valueInputField.SetTextWithoutNotify(_schemeVariable.GetStartValue().ToString());
            }

            _chooseButton.gameObject.SetActive(_variableList.HasSender);
        }

        private void InitializeValuePlaceHolder(int value)
        {
            _valueInputField.inputValidator = null;
            _valueInputField.text = null;

            switch (value)
            {
                case 0:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.Digit;
                    _valueInputFieldPlaceHolder.text = _intPlaceHolder;
                    _variableType = typeof(int);
                    break;
                case 1:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                    _valueInputFieldPlaceHolder.text = _floatPlaceHolder;
                    _variableType = typeof(float);
                    break;
                case 2:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.None;
                    _valueInputFieldPlaceHolder.text = _stringPlaceHolder;
                    _variableType = typeof(string);
                    break;
                case 3:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
                    _valueInputField.inputValidator = _boolValidator;
                    _valueInputFieldPlaceHolder.text = _boolPlaceHolder;
                    _variableType = typeof(bool);
                    break;
            }
        }
        #endregion

        public void OnEndEdit()
        {
            if (_variableType == null || string.IsNullOrEmpty(_variableName) || _variableValue == null) return;

            if (_variableType == typeof(int))
                _schemeVariable = new SchemeVariable<int>(_variableName);
            else if (_variableType == typeof(float))
                _schemeVariable = new SchemeVariable<float>(_variableName);
            else if (_variableType == typeof(string))
                _schemeVariable = new SchemeVariable<string>(_variableName);
            else if (_variableType == typeof(bool))
                _schemeVariable = new SchemeVariable<bool>(_variableName);

            _schemeVariable.SetStartValue(_variableValue);

            if (_schemeVariable != null)
                _variableList.AddOrModifyVariable(this);
        }

        public void OnDestroy()
        {
            _nameInputField.onEndEdit.RemoveAllListeners();
            _typeDropdown.onValueChanged.RemoveAllListeners();
            _valueInputField.onEndEdit.RemoveAllListeners();
            _chooseButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
        }
    }
}
