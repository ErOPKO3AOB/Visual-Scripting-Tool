using Session.Scheme.Connector;
using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class StartBlock : IActionProvider, IDisposable
    {
        public StartBlock(SchemeBlockFacade facade, BlockConnector connector)
        {
            _facade = facade;
            _connector = connector;
        }

        private readonly SchemeBlockFacade _facade;
        private readonly BlockConnector _connector;

        bool IActionProvider.CanMoveHere { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IActionProvider IActionProvider.Next { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        void IDisposable.Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }

        public void ProvideAction()
        {
            throw new NotImplementedException();
        }

        void IActionProvider.ProvideAction()
        {
            throw new NotImplementedException();
        }
    }
}