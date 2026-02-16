using Extensions;
using Session.Scheme.Variables;
using System;
using System.Collections.Generic;
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

        public SchemeVariableBase SchemeVariable { get; set; }

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

            _nameInputField.onEndEdit.AddListener((string text) => { _variableName = text; OnEndEdit(); });
            _typeDropdown.onValueChanged.AddListener((int value) => { InitializeValuePlaceHolder(value); OnEndEdit(); });
            _valueInputField.onEndEdit.AddListener((string value) => { _variableValue = value; OnEndEdit(); });

            _chooseButton.onClick.AddListener(() => { OnEndEdit(); _variableList.ChooseVariable(_variableName); });
            _deleteButton.onClick.AddListener(() =>
            {
                _variableList.RemoveVariable(this);
            });
        }

        private void RebuildUI()
        {
            _typeDropdown.ClearOptions();
            _typeDropdown.AddOptions(new List<TMP_Dropdown.OptionData>() {
                new(text: "int"),
                new(text: "float"),
                new(text: "string"),
                new(text: "bool"),
            });

            if (_schemeVariable != null && _schemeVariable.variableName != null
                && _schemeVariable.ValueType != null && _schemeVariable.GetValue() != null)
            {
                _nameInputField.SetTextWithoutNotify(_schemeVariable.variableName);
                _valueInputField.SetTextWithoutNotify(_schemeVariable.GetValue().ToString());
                _typeDropdown.value = _variableService.GetTypeIntegerValue(_schemeVariable.ValueType);
            }
        }

        private void InitializeValuePlaceHolder(int value)
        {
            _valueInputField.inputValidator = null;
            _valueInputField.text = null;

            switch (value)
            {
                case 0:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.Digit;
                    _valueInputField.text = "0";
                    _valueInputFieldPlaceHolder.text = _intPlaceHolder;
                    _variableType = typeof(int);
                    break;
                case 1:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                    _valueInputField.text = "0";
                    _valueInputFieldPlaceHolder.text = _floatPlaceHolder;
                    _variableType = typeof(float);
                    break;
                case 2:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.None;
                    _valueInputField.text = " ";
                    _valueInputFieldPlaceHolder.text = _stringPlaceHolder;
                    _variableType = typeof(string);
                    break;
                case 3:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
                    _valueInputField.inputValidator = _boolValidator;
                    _valueInputField.text = "false";
                    _valueInputFieldPlaceHolder.text = _boolPlaceHolder;
                    _variableType = typeof(bool);
                    break;
            }
        }
        #endregion

        public void OnEndEdit()
        {
            if (_variableType == null || _variableName == null || _variableValue == null) return;
            Debug.Log($"{_variableType} {_variableName} {_variableValue}");

            if (_variableType == typeof(int))
                _schemeVariable = new SchemeVariable<int>(_variableName);
            else if (_variableType == typeof(float))
                _schemeVariable = new SchemeVariable<float>(_variableName);
            else if (_variableType == typeof(string))
                _schemeVariable = new SchemeVariable<string>(_variableName);
            else if (_variableType == typeof(bool))
                _schemeVariable = new SchemeVariable<bool>(_variableName);

            _schemeVariable.SetValue(_variableValue);

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
