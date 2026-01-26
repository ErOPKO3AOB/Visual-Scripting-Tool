using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using UnityEngine;
using UnityEngine.UI;

namespace Session.Scheme.Windows
{
    public class OperationItem : BaseWindowUI
    {
        [Header("UI")]
        [SerializeField] private ChoosedVariableItem _operand1;
        [SerializeField] private Dropdown _operationDropDown;
        [SerializeField] private ChoosedVariableItem _operand2;

        private MethodBlock _methodBlock;

        private void Start()
        {
            
        }

        private void SendOperationToMethodBlock()
        {
            _methodBlock.
        }
    }
}