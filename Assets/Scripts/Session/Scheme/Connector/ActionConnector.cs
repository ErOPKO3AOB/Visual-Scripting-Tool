using Session.Scheme.Block;
using UnityEngine;

namespace Session.Scheme.Connector
{
    public class ActionConnector
    {
        public ActionConnector(IBlock firstProvider)
        {
            _firstProvider = firstProvider;
        }

        private readonly IBlock _firstProvider;

        public void Connect(IBlock secondProvider)
        {
            _firstProvider.Next = secondProvider;
            
            var childFacade = ((IBlock)_firstProvider.Next).Facade;
            childFacade.DraggableBlockButton.SetDragUsage(false);
            childFacade.transform.SetParent(_firstProvider.Facade.transform);
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