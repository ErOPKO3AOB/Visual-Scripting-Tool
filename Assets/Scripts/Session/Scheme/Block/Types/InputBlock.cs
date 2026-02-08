using Session.Scheme.Variables;
using Session.Scheme.Windows;
using System;
using System.Collections;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class InputBlock : IBlock, IDisposable
    {
        public InputBlock(SchemeBlockFacade facade, VariableService variableService)
        {
            _facade = facade;
            
            _consoleWindow = GameObject.FindAnyObjectByType<ConsoleWindow>();
            _variableService = variableService;
        }

        private readonly SchemeBlockFacade _facade;
        public SchemeBlockFacade Facade => _facade;

        private readonly ConsoleWindow _consoleWindow;
        private readonly VariableService _variableService;

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
            _consoleWindow.GetInput(VariableName, this);

            while (!_used)
            {
                yield return null;
            }

            Next.ProvideAction();
        }

        public void SetInput(object value)
        {
            _variableService.SetValueToVariable(VariableName, value);

            _used = true;
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}