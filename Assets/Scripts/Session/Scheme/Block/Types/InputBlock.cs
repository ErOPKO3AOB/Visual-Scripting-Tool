using Session.Scheme.Variables;
using Session.Scheme.Windows;
using System;
using System.Collections;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class InputBlock : IActionProvider, IDisposable
    {
        public InputBlock(SchemeBlockFacade facade, VariableService variableService, WindowService windowService)
        {
            _facade = facade;
            _consoleWindow = GameObject.FindAnyObjectByType<ConsoleWindow>();
            _variableService = variableService;
            _windowService = windowService;
            _windowService.OnOpenWindow += OnOpenWindow;
        }

        private readonly SchemeBlockFacade _facade;
        private readonly ConsoleWindow _consoleWindow;
        private readonly VariableService _variableService;
        private readonly WindowService _windowService;

        public IActionProvider Next { get => _next; set => _next = value; }
        private IActionProvider _next;

        public string VariableName { get; set; }

        private bool _used = false;

        private void OnOpenWindow(BaseWindow window)
        {
            if (window.GetType() != typeof(InputWindowUI)) return;

            InputWindowUI inputWindow = (InputWindowUI)window;
            inputWindow.Block = this;
        }

        public void ProvideAction()
        {
            _facade.StartCoroutine(WaitInput());
        }

        private IEnumerator WaitInput()
        {
            _consoleWindow.GetInput(VariableName, this);

            while (true)
            {
                yield return null;

                if (_used == true)
                {
                    break;
                }
            }

            _next.ProvideAction();
        }

        public void SetInput(object value)
        {
            _variableService.SetValueToVariable(VariableName, value);

            _used = true;
        }

        public void Dispose()
        {
            _windowService.OnOpenWindow -= OnOpenWindow;
            GameObject.Destroy(_facade.gameObject);
        }
    }
}