using Extensions;
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
        [SerializeField] private Button _deleteButton;

        public SchemeVariableBase SchemeVariable { get; private set; }

        public void Initialize(VariablePickerItem variablePickerUI, SchemeVariableBase schemeVariable)
        {
            SchemeVariable = schemeVariable;

            _deleteButton.onClick.AddListener(() => { variablePickerUI.DeleteVariable(); });

            _typeText.text = TypeExtensions.GetFriendlyTypeName(SchemeVariable.ValueType);
            _nameText.text = SchemeVariable.variableName.ToString();
        }

        private void OnDestroy()
        {
            _deleteButton.onClick.RemoveAllListeners();
        }
    }
}