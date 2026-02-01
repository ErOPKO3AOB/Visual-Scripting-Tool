using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
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
        public TextMeshProUGUI OutputText { get { return _outputText; } }
        [SerializeField] private TMP_InputField _inputField;
        public TMP_InputField InputField { get { return _inputField; } }

        public void BuildInputMessage(string variableName, InputBlock inputBlock)
        {
            // Making input field send value, then deactivate
            _inputField.gameObject.SetActive(true);
            _inputField.onSubmit.AddListener((string value) =>
            {
                int varIndex = _variableService.CheckExistance(variableName);
                if (varIndex > -1) _variableService.Variables[varIndex].SetValue(value);
                inputBlock.SetInput(value);
                _inputField.interactable = false;
                _inputField.onSubmit.RemoveAllListeners();
            });

            _outputText.gameObject.SetActive(true);
            _outputText.text = $"{variableName}:";
        }

        public void BuildOutputMessage(string message)
        {
            _inputField.gameObject.SetActive(false);
            _outputText.gameObject.SetActive(true);
            _outputText.text = message;
        }
    }
}