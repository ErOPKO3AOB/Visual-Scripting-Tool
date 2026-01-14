using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class EndBlock : IActionProvider, IDisposable
    {
        public EndBlock(SchemeBlockFacade facade)
        {
            _facade = facade;
        }

        private readonly SchemeBlockFacade _facade;

        public IActionProvider Next { get => _next; set => _next = value; }
        private IActionProvider _next;

        public void ProvideAction()
        {
            Next?.ProvideAction();
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}