using Extensions;
using Session.Scheme.Variables;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class VariableItemUI : BaseWindow
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

        private Type _variableType;
        private string _variableName;
        private object _variableValue;

        private VariableListWindow _variableList;
        private SchemeVariableBase _schemeVariable;

        public SchemeVariableBase SchemeVariable => _schemeVariable;

        public override void SetSender(object sender)
        {
            try
            {
                _variableList = (VariableListWindow)sender;
                Initialize();
                RebuildUI();
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        #region Initiallization
        private void Initialize()
        {
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
            if (_schemeVariable == null || _schemeVariable.variableName == null || _schemeVariable.ValueType == null || _schemeVariable.GetValue() == null) return;

            _nameInputField.SetTextWithoutNotify(_schemeVariable.variableName);
            InitializeValuePlaceHolder(_variableService.GetTypeIntegerValue(_schemeVariable.ValueType));
            _valueInputField.SetTextWithoutNotify(_schemeVariable.GetValue().ToString());
            _typeDropdown.value = _variableService.GetTypeIntegerValue(_schemeVariable.ValueType);
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
                    break;
                case 1:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                    _valueInputField.text = "0";
                    _valueInputFieldPlaceHolder.text = _floatPlaceHolder;
                    break;
                case 2:
                    _valueInputField.characterValidation = TMP_InputField.CharacterValidation.None;
                    _valueInputField.text = " ";
                    _valueInputFieldPlaceHolder.text = _stringPlaceHolder;
                    break;
                case 3:
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
            _variableList.AddOrModifyVariable(_schemeVariable);
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
