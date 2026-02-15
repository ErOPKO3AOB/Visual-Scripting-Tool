using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class EndBlock : IBlock, IDisposable
    {
        public EndBlock(SchemeBlockFacade facade)
        {
            _facade = facade;
        }

        private readonly SchemeBlockFacade _facade;
        public SchemeBlockFacade Facade => _facade;

        public IActionProvider Next { get; set; }
        public bool SingleInstance { get => _facade.SingleInstance; }

        public void ProvideAction()
        {
            Next?.ProvideAction();
        }

        public bool CheckForCorrectRelationships()
        {
            return true;
        }

        public bool CheckForCorrectValues()
        {
            return true;
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}