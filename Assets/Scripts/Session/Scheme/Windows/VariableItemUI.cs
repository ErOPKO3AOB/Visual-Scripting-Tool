using GlobalServices.InputField;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Session.Scheme.Windows
{
    public class VariableItemUI : BaseWindowUI
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_Dropdown _typeDropdown;
        [SerializeField] private TMP_InputField _valueInputField;
        [SerializeField] private TMP_Text _valueInputFieldPlaceHolder;
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

        private Type _variableType;
        private string _variableName;
        private object _variableValue;

        private VariableListWindowUI _masterList;
        public VariableListWindowUI MasterList
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
            _nameInputField.onSubmit.AddListener((string text) => { _variableName = text; OnEndEdit(); });
            _typeDropdown.onValueChanged.AddListener((int value) => { InitializeValuePlaceHolder(value); OnEndEdit(); });
            _valueInputField.onSubmit.AddListener((string value) => { _variableValue = value; OnEndEdit(); });
            _deleteButton.onClick.AddListener(() => { _masterList.RemoveVariable(this); });

            InitializeValuePlaceHolder(_typeDropdown.value);
        }

        public void OnDestroy()
        {
            _nameInputField.onSubmit.RemoveAllListeners();
            _typeDropdown.onValueChanged.RemoveAllListeners();
            _valueInputField.onSubmit.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
        }

        public void RebuildUI(int typeValue, string name, object value)
        {
            _typeDropdown.value = typeValue;
            _nameInputField.text = name;
            _valueInputField.text = value.ToString();
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
                    _valueInputFieldPlaceHolder.text = _intPlaceHolder;
                    break;
                case 1:
                    _variableType = typeof(float);
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                    _valueInputFieldPlaceHolder.text = _floatPlaceHolder;
                    break;
                case 2:
                    _variableType = typeof(string);
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.None;
                    _valueInputFieldPlaceHolder.text = _stringPlaceHolder;
                    break;
                case 3:
                    _variableType = typeof(bool);
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
                    _valueInputField.inputValidator = new BoolValidator();
                    _valueInputFieldPlaceHolder.text = _boolPlaceHolder;
                    break;
            }
        }
        #endregion

        public void OnEndEdit()
        {
            _masterList.AddVariable(_variableName, _variableType, _variableValue);
            Debug.Log("On end edit");
        }
    }
}
