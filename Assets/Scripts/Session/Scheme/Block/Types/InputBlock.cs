using Session.Scheme.Variables;
using Session.Scheme.Windows;
using System;
using System.Collections;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class InputBlock : IBlock, IDisposable
    {
        public InputBlock(SchemeBlockFacade facade, VariableService variableService, SchemeConsoleService consoleService)
        {
            _facade = facade;
            _variableService = variableService;
            _consoleService = consoleService;
        }

        private readonly SchemeBlockFacade _facade;
        public SchemeBlockFacade Facade => _facade;

        private readonly ConsoleWindow _consoleWindow;
        private readonly VariableService _variableService;
        private readonly SchemeConsoleService _consoleService;

        private IBlock _nextBlock;

        public IActionProvider Next { get => _nextBlock; set => _nextBlock = (IBlock)value; }
        public bool SingleInstance { get => _facade.SingleInstance; }
        private SchemeVariableBase _schemeVariable;

        public SchemeVariableBase SchemeVariable => _schemeVariable;

        private bool _used = false;

        public void SetOperation(SchemeVariableBase variableToInputRequest)
        {
            _schemeVariable = variableToInputRequest;

            Facade.Label.SetText($"¬вод: {SchemeVariable.variableName}");
        }

        public void ProvideAction()
        {

            _used = false;

            _facade.StartCoroutine(WaitInput());
        }

        private IEnumerator WaitInput()
        {
            _consoleService.SpawnInputRequest(SchemeVariable.variableName, this);

            while (!_used)
            {
                yield return null;
            }

            Next?.ProvideAction();
        }

        public void SetInput(object value)
        {
            Debug.Log("GOT INPUT!!!");

            _variableService.SetValueToVariable(SchemeVariable.variableName, value);

            _used = true;
        }

        public bool CheckForCorrectRelationships()
        {
            return Next != null && _nextBlock.CheckForCorrectRelationships();
        }

        public bool CheckForCorrectValues()
        {
            return (Next == null || _nextBlock.CheckForCorrectValues());
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}