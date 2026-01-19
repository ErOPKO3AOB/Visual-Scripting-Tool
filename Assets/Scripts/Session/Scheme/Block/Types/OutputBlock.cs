using Session.Scheme.Variables;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Session.Scheme.Block.Types
{
    public class OutputBlock : IActionProvider, IDisposable
    {
        public OutputBlock(SchemeBlockFacade facade)
        {
            _facade = facade;
        }

        private readonly SchemeBlockFacade _facade;

        public IActionProvider Next { get => _next; set => _next = value; }
        private IActionProvider _next;

        public List<SchemeVariableBase> SchemeVariables { get; set; }

        public void ProvideAction()
        {
            //SchemeConsole.SetOutput(SchemeVariable);
            _next.ProvideAction();
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}