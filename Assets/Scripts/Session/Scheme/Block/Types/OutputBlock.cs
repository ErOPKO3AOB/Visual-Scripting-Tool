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
        public SchemeBlockFacade Facade => _facade;

        private readonly SchemeConsoleService _consoleService;

        public IBlock Next { get; set; }
        public bool SingleInstance => _facade.SingleInstance;

        private SchemeVariableBase _schemeVariable;

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
            return Next != null && Next.CheckForCorrectRelationships();
        }

        public bool CheckForCorrectValues()
        {
            return (Next == null || Next.CheckForCorrectValues());
        }


        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}