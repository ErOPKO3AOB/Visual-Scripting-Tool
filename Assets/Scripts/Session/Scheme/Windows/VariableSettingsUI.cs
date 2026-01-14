using GlobalServices.InputField;
using Session.Scheme.Variables;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Session.Scheme.Windows
{
    public class VariableSettingsUI : SettingsBaseWindowUI, IInitializable, IDisposable
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_Dropdown _typeDropdown;
        [SerializeField] private TMP_InputField _valueInputField;
        [SerializeField] private TMP_Text _valueInputFieldPlaceHolder;
        [SerializeField] private Button _deleteButton;

        [Header("Value input validation")]
        [Header("Int")]
        [SerializeField] private string _intPlaceHolder = "Введите целое число";
        [Header("Float")]
        [SerializeField] private string _floatPlaceHolder = "Введите дробное число";
        [Header("String")]
        [SerializeField] private string _stringPlaceHolder = "Введите строку";
        [Header("Bool")]
        [SerializeField] private string _boolPlaceHolder = "Введите булевое значение true/false";

        private VariableService _variableService;


        private string _variableName;
        private Type _variableType;
        private object _variableValue;

        private void Start()
        {
            Initialize();
        }

        #region Initiallization
        [Inject]
        public void Construct(VariableService variableService)
        {
            _variableService = variableService;
        }

        public void Initialize()
        {
            _nameInputField.onSubmit.AddListener((string text) => { _variableName = text; OnEndEdit(); });
            _typeDropdown.onValueChanged.AddListener((int value) => { InitializeValuePlaceHolder(value); OnEndEdit(); });
            _valueInputField.onSubmit.AddListener((string value) => { _variableValue = value; OnEndEdit(); });
            _deleteButton.onClick.AddListener(() => { _variableService.RemoveVariable(_variableName); Destroy(gameObject); });

            InitializeValuePlaceHolder(_typeDropdown.value);
        }

        public void Dispose()
        {
            _nameInputField.onSubmit.RemoveAllListeners();
            _typeDropdown.onValueChanged.RemoveAllListeners();
            _valueInputField.onSubmit.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
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

        public override void OnEndEdit()
        {
            if (_variableName != null && _variableType != null)
            {
                switch (_variableType)
                {
                    case Type intType when intType == typeof(int):
                        _variableService.BuildVariable<int>(_variableName, _variableValue);
                        break;
                    case Type floatType when floatType == typeof(float):
                        _variableService.BuildVariable<float>(_variableName, _variableValue);
                        break;
                    case Type stringType when stringType == typeof(string):
                        _variableService.BuildVariable<string>(_variableName, _variableValue);
                        break;
                    case Type boolType when boolType == typeof(bool):
                        _variableService.BuildVariable<bool>(_variableName, _variableValue);
                        break;
                }
            }

            else
            {
                Debug.LogAssertion("Не указан один из важных параметров для создания переменной!");
            }
        }
    }
}
