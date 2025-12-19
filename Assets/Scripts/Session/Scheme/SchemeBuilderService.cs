using Session.Scheme.Block;
using Session.Scheme.Connector;
using System;
using System.Collections.Generic;

namespace Session.Scheme
{
    public class SchemeBuilderService : IDisposable
    {
        private List<ISchemeBlock> _blocks;
        private List<BlockConnector> _connectors;

        public void SpawnBlock()
        {

        }

        public void ConnectBlocksWithConnector(ISchemeBlock outputPoint, ISchemeBlock inputPoint, BlockConnector connector)
        {
            connector.SetInputConnection(outputPoint);
            connector.SetOutputConnection(inputPoint);
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