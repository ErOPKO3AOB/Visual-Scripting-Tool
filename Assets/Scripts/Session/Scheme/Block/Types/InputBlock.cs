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

        public IBlock Next { get; set; }
        public bool SingleInstance { get => _facade.SingleInstance; }


        public string VariableName { get; set; }

        private bool _used = false;

        public void ProvideAction()
        {
            _used = false;

            _facade.StartCoroutine(WaitInput());
        }

        private IEnumerator WaitInput()
        {
            _consoleService.SpawnInputRequest(VariableName, this);

            while (!_used)
            {
                yield return null;
            }

            Next?.ProvideAction();
        }

        public void SetInput(object value)
        {
            _variableService.SetValueToVariable(VariableName, value);

            _used = true;
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