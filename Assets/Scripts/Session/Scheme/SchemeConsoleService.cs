using Session.Scheme.Block.Types;
using Session.Scheme.Windows;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Session.Scheme
{
    public class SchemeConsoleService
    {
        public List<string> Messages { get; private set; } = new();

        public UnityAction<string> OnMessageSpawn;
        public UnityAction<string, InputBlock> OnInputRequest;
    
        public void SpawnMessage(string message)
        {
            Messages.Add(message);

            OnMessageSpawn?.Invoke(message);
        }

        public void SpawnInputRequest(string varName, InputBlock inputBlock)
        {
            OnInputRequest?.Invoke(varName, inputBlock);
        }
    }
}