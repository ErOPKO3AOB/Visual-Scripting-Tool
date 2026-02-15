using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class StartBlock : IBlock, IDisposable
    {
        public StartBlock(SchemeBlockFacade facade)
        {
            _facade = facade;
        }

        private readonly SchemeBlockFacade _facade;
        public SchemeBlockFacade Facade => _facade;

        private IBlock _nextBlock;

        public IActionProvider Next { get => _nextBlock; set => _nextBlock = (IBlock)value; }
        public bool SingleInstance { get => _facade.SingleInstance; }


        public void ProvideAction()
        {
            Next?.ProvideAction();
        }

        public bool CheckForCorrectRelationships()
        {
            return Next != null && _nextBlock.CheckForCorrectRelationships();
        }

        public bool CheckForCorrectValues()
        {
            return (Next == null || _nextBlock.CheckForCorrectValues());
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}