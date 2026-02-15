using Session.Scheme.Variables;
using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class OutputBlock : IBlock, IDisposable
    {
        public OutputBlock(SchemeBlockFacade facade, SchemeConsoleService consoleService)
        {
            _facade = facade;
            _consoleService = consoleService;
        }

        private readonly SchemeBlockFacade _facade;
        private readonly SchemeConsoleService _consoleService;

        private SchemeVariableBase _schemeVariable;

        private IBlock _nextBlock;

        public IActionProvider Next { get => _nextBlock; set => _nextBlock = (IBlock)value; }
        public bool SingleInstance => _facade.SingleInstance;
        public SchemeBlockFacade Facade => _facade;
        public SchemeVariableBase SchemeVariable => _schemeVariable;

        public void SetOperation(SchemeVariableBase variableToOutputRequest)
        {
            _schemeVariable = variableToOutputRequest;

            Facade.Label.SetText($"Вывод: {_schemeVariable.variableName}");
        }

        public void ProvideAction()
        {
            string message = "";

            //for (int i = 0; i < SchemeVariable.Count; i++)
            //{
            //    message += SchemeVariable[i].GetValue().ToString();
            //}

            message = SchemeVariable.GetValue().ToString();

            _consoleService.SpawnMessage(message);
            Next?.ProvideAction();
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