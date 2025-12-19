using Session.Scheme.Connector;
using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class StartBlock : ISchemeBlock, IDisposable
    {
        public StartBlock(SchemeBlockFacade facade, BlockConnector connector)
        {
            _facade = facade;
            _connector = connector;
        }

        private readonly SchemeBlockFacade _facade;
        private readonly BlockConnector _connector;

        void ISchemeBlock.SendInput()
        {
            //_connector.BurpOutput();

        }

        void IDisposable.Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}