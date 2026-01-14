using Session.Scheme.Block;
using Session.Scheme.Connector;
using System;
using System.Collections.Generic;

namespace Session.Scheme
{
    public class SchemeBuilderService : IDisposable
    {
        private List<IActionProvider> _blocks;
        private List<ActionConnector> _connectors;

        public void SpawnBlock()
        {

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