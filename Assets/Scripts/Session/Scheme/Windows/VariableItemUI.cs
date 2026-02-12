using Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Session.Scheme.Windows
{
    public class VariableItemUI : BaseWindow
    {
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

        private VariableListWindow _masterList;
        private Type _variableType = typeof(int);
        private string _variableName;
        private object _variableValue = null;

        public string VariableName => _variableName;

        public VariableListWindow MasterList
        {
            get
            {
                return _masterList;
            }
            set
            {
                _masterList = value;
                Initialize();
            }
        }

        #region Initiallization
        private void Initialize()
        {
            _nameInputField.onEndEdit.AddListener((string text) => { _variableName = text; OnEndEdit(); });
            _typeDropdown.onValueChanged.AddListener((int value) => { InitializeValuePlaceHolder(value); OnEndEdit(); });
            _valueInputField.onEndEdit.AddListener((string value) => { _variableValue = value; OnEndEdit(); });
            _chooseButton.onClick.AddListener(() => { OnEndEdit(); _masterList.ChooseVariable(_variableName); });
            _deleteButton.onClick.AddListener(() =>
            {
                _masterList.RemoveVariable(this);
            });

            InitializeValuePlaceHolder(_typeDropdown.value);
        }

        public void RebuildUI(int typeValue, string name, object value)
        {
            _typeDropdown.value = typeValue;
            _nameInputField.text = name;
            _valueInputField.text = value?.ToString();
        }

        private void InitializeValuePlaceHolder(int value)
        {
            _valueInputField.inputValidator = null;
            _valueInputField.text = null;

            switch (value)
            {
                case 0:
                    _variableType = typeof(int);
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.Digit;
                    _valueInputField.text = "0";
                    _valueInputFieldPlaceHolder.text = _intPlaceHolder;
                    break;
                case 1:
                    _variableType = typeof(float);
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                    _valueInputField.text = "0";
                    _valueInputFieldPlaceHolder.text = _floatPlaceHolder;
                    break;
                case 2:
                    _variableType = typeof(string);
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.None;
                    _valueInputField.text = " ";
                    _valueInputFieldPlaceHolder.text = _stringPlaceHolder;
                    break;
                case 3:
                    _variableType = typeof(bool);
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
                    _valueInputField.inputValidator = _boolValidator;
                    _valueInputField.text = "false";
                    _valueInputFieldPlaceHolder.text = _boolPlaceHolder;
                    break;
            }
        }
        #endregion

        public void OnEndEdit()
        {
            _masterList.AddVariable(_variableName, _variableType, _variableValue);
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
