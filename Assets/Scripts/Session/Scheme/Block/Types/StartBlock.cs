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

        public IBlock Next { get; set; }
        public bool SingleInstance { get => _facade.SingleInstance; }


        public void ProvideAction()
        {
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