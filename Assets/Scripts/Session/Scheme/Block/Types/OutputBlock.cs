using Session.Scheme.Variables;
using Session.Scheme.Windows;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class OutputBlock : IActionProvider, IDisposable
    {
        public OutputBlock(SchemeBlockFacade facade)
        {
            _facade = facade;
            _consoleWindow = GameObject.FindAnyObjectByType<ConsoleWindow>();
        }

        private readonly SchemeBlockFacade _facade;
        private readonly ConsoleWindow _consoleWindow;

        public IActionProvider Next { get => _next; set => _next = value; }
        private IActionProvider _next;

        public List<SchemeVariableBase> SchemeVariables { get; set; }

        public void ProvideAction()
        {
            string message = "";

            for (int i = 0; i < SchemeVariables.Count; i++)
            {
                message += SchemeVariables[i].GetValue().ToString();
            }

            _consoleWindow.SpawnOutuptMessage(message);
            _next.ProvideAction();
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}