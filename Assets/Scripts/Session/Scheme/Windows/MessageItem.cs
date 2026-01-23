using TMPro;
using UnityEngine;

namespace Session.Scheme.Windows
{
    public class MessageItem : BaseWindowUI
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _outputText;
        [SerializeField] private TMP_InputField _inputField;
        
        public void BuildInputMessage()
        {
            _inputField.gameObject.SetActive(true);
            _outputText.gameObject.SetActive(false);
        }
        
        public void BuildOutputMessage(string message)
        {
            _inputField.gameObject.SetActive(false);
            _outputText.gameObject.SetActive(true);
            _outputText.text = message;
        }
    }
}