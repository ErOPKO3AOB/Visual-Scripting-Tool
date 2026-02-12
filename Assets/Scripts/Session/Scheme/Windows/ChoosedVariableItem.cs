using Session.Scheme.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Session.Scheme.Windows
{
    public class ChoosedVariableItem : BaseWindow
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _typeText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private Button _deleteButton;

        public SchemeVariableBase SchemeVariable { get; private set; }

        public void Initialize(VariablePickerUI variablePickerUI, SchemeVariableBase schemeVariable)
        {
            SchemeVariable = schemeVariable;

            _deleteButton.onClick.AddListener(() => { variablePickerUI.OnVariableDelete(); });

            _typeText.text = SchemeVariable.ValueType.ToString();
            _nameText.text = SchemeVariable.variableName.ToString();
            _valueText.text = SchemeVariable.GetValue().ToString();
        }

        private void OnDestroy()
        {
            _deleteButton.onClick.RemoveAllListeners();
        }
    }
}