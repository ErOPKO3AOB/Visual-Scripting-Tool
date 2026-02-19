using Extensions;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using TMPro;
using UnityEngine;
using VContainer;

namespace Session.Scheme.Windows
{
    public class MessageItem : BaseWindow
    {
        [Inject]
        public void Construct(VariableService variableService)
        {
            _variableService = variableService;
        }

        private VariableService _variableService;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _outputText;
        [SerializeField] private TextMeshProUGUI _placeholderText;
        [SerializeField] private BoolValidator _boolValidator;
        public TextMeshProUGUI OutputText { get { return _outputText; } }
        [SerializeField] private TMP_InputField _inputField;
        public TMP_InputField InputField { get { return _inputField; } }

        public void BuildInputMessage(string variableName, InputBlock inputBlock)
        {
            // Making input field send value, then deactivate
            _inputField.gameObject.SetActive(true);

            Type variableType = _variableService.Variables.Find(v => v.variableName == variableName).GetType();

            if (variableType == typeof(int))
            {
                _inputField.characterValidation = TMP_InputField.CharacterValidation.Digit;
                _placeholderText.text = "¬ведите целое число";
            }

            else if (variableType == typeof(float))
            {
                _inputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                _placeholderText.text = "¬ведите дробное число";
            }

            else if (variableType == typeof(string))
            {
                _inputField.characterValidation = TMP_InputField.CharacterValidation.None;
                _placeholderText.text = "¬ведите строку";
            }

            else if (variableType == typeof(bool))
            {
                _inputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
                _inputField.inputValidator = _boolValidator;
                _placeholderText.text = "¬ведите булевое значение true/false";
            }

            _inputField.onSubmit.AddListener((UnityEngine.Events.UnityAction<string>)((string value) =>
            {
                int varIndex = _variableService.CheckVariableExistance(variableName);
                if (varIndex > -1)
                {
                    _variableService.Variables[varIndex].SetValue((object)value);
                    inputBlock.SetInput(value);
                    _inputField.interactable = false;
                }

                _inputField.onSubmit.RemoveAllListeners();
            }));

            _outputText.gameObject.SetActive(true);
            _outputText.text = $"¬ведите {variableName}:";
        }

        public void BuildOutputMessage(string message)
        {
            _inputField.gameObject.SetActive(false);
            _outputText.gameObject.SetActive(true);
            _outputText.text = message;
        }
    }
}