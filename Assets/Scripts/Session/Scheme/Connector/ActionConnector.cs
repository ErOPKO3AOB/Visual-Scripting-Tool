using Session.Scheme.Block;
using UnityEngine;

namespace Session.Scheme.Connector
{
    public class ActionConnector
    {
        public ActionConnector(IActionProvider firstProvider)
        {
            _firstProvider = firstProvider;
        }

        private readonly IActionProvider _firstProvider;

        public void Connect(IActionProvider secondProvider, int outputIndex)
        {
            if (_firstProvider is IBlock block)
                block.CurrentOutputIndex = outputIndex;

            _firstProvider.Next = secondProvider;

            var childFacade = ((IBlock)_firstProvider.Next).Facade;
            childFacade.DraggableBlockButton.SetDragUsage(false);
            childFacade.transform.SetParent(((IBlock)_firstProvider).Facade.transform);
            childFacade.Collider.enabled = false;
            childFacade.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        public void Disconnect()
        {
            if (_firstProvider.Next == null) return;

            var childFacade = ((IBlock)_firstProvider.Next).Facade;
            childFacade.transform.SetParent(null);
            childFacade.DraggableBlockButton.SetDragUsage(true);
            childFacade.Collider.enabled = true;
            childFacade.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _firstProvider.Next = null;
        }
    }
}