using Cysharp.Threading.Tasks;
using Session.Scheme.Variables;
using System;
using System.Threading;
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

        private readonly VariableService _variableService;
        private readonly SchemeConsoleService _consoleService;

        private int _currentOutputIndex;

        public IBlock.BlockType ConcreteType { get => IBlock.BlockType.Input; }
        public SchemeBlockFacade Facade => _facade;
        public int CurrentOutputIndex { get { return _currentOutputIndex; } set { _currentOutputIndex = 0; } }

        private IBlock _nextBlock;

        public IActionProvider Next { get => _nextBlock; set => _nextBlock = (IBlock)value; }
        public bool SingleInstance { get => _facade.SingleInstance; }
        private SchemeVariableBase _schemeVariable;

        public SchemeVariableBase SchemeVariable => _schemeVariable;

        private bool _used = false;
        private CancellationTokenSource _cts;

        public void SetOperation(SchemeVariableBase variableToInputRequest)
        {
            _schemeVariable = variableToInputRequest;

            string displayName = variableToInputRequest != null ?
                $"Input: {_schemeVariable.variableName}"
                : "Values not set!";

            _facade.Label.SetText(displayName);
        }

        public void ProvideAction()
        {
            _used = false;

            _cts = new CancellationTokenSource();

            WaitInput().Forget();
        }

        private async UniTask WaitInput()
        {
            _consoleService.SpawnInputRequest(SchemeVariable.variableName, this);

            while (!_used)
            {
                await UniTask.Yield(cancellationToken: _cts.Token);
            }

            Next?.ProvideAction();
        }

        public void SetInput(object value)
        {
            _variableService.SetValueToVariable(SchemeVariable.variableName, value);

            _used = true;
        }

        public bool CheckForCorrectRelationships()
        {
            return Next != null && _nextBlock.CheckForCorrectRelationships();
        }

        public bool CheckForCorrectValues()
        {
            return _schemeVariable != null && (Next == null || _nextBlock.CheckForCorrectValues());
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();

            GameObject.Destroy(_facade.gameObject);
        }
    }
}