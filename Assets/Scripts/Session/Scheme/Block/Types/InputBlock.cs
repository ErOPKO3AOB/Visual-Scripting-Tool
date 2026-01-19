using Session.Scheme.Variables;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Session.Scheme.Block.Types
{
    public class InputBlock : IActionProvider, IDisposable
    {
        public InputBlock(SchemeBlockFacade facade)
        {
            _facade = facade;
        }

        private readonly SchemeBlockFacade _facade;

        public IActionProvider Next { get => _next; set => _next = value; }
        private IActionProvider _next;

        public List<SchemeVariableBase> SchemeVariables { get; set; }

        public void ProvideAction()
        {
            //SchemeVariable = SchemeConsole.GetInput();
            _next.ProvideAction();
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}