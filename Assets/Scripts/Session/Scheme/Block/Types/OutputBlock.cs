using Session.Scheme.Variables;
using Session.Scheme.Windows;
using System;
using System.Collections.Generic;
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
        public bool SingleInstance { get => _facade.SingleInstance; }


        public List<SchemeVariableBase> SchemeVariables { get; set; }

        public void ProvideAction()
        {
            string message = "";

            for (int i = 0; i < SchemeVariables.Count; i++)
            {
                message += SchemeVariables[i].GetValue().ToString();
            }

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