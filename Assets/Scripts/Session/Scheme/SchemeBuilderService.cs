using Session.Scheme.Block;
using Session.Scheme.Connector;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace Session.Scheme
{
    public class SchemeBuilderService : IDisposable, IInitializable
    {
        public SchemeBuilderService(Func<string, Transform, SchemeBlockFacade> factory)
        {
            _factory = factory;
        }

        private readonly Func<string, Transform, SchemeBlockFacade> _factory;

        private List<IActionProvider> _blocks = new();
        private List<ActionConnector> _connectors = new();

        public void Initialize()
        {
            SpawnBlock("METHOD_BLOCK");
        }

        public void SpawnBlock(string blockName)
        {
            _factory.Invoke(blockName, null);
        }

        public void DestroyBlock(string blockName)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                // Нужна инерфейсовая логика для блока типа IBlock
            }
        }

        public void ConnectBlocksWithConnector(IActionProvider outputPoint, IActionProvider inputPoint, ActionConnector connector)
        {
            //connector.SetInputConnection(outputPoint);
            //connector.SetOutputConnection(inputPoint);
        }

        public void SpawnConnector()
        {

        }

        void IDisposable.Dispose()
        {
            _blocks.Clear();
            _connectors.Clear();
        }
    }
}