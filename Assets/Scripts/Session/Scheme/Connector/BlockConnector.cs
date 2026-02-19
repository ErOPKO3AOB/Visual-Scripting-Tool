using Session.Scheme.Block;
using UnityEngine;

namespace Session.Scheme.Connector
{
    public class BlockConnector
    {
        public BlockConnector(IBlock firstProvider)
        {
            _firstProvider = firstProvider;
        }

        private readonly IBlock _firstProvider;

        public void Connect(IBlock secondProvider, int outputIndex)
        {
            _firstProvider.CurrentOutputIndex = outputIndex;
            _firstProvider.Next = secondProvider;

            secondProvider.Facade.DraggableBlockButton.SetDragUsage(false);
            secondProvider.Facade.transform.SetParent(_firstProvider.Facade.transform);
            secondProvider.Facade.Collider.enabled = false;
            secondProvider.Facade.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        public void Disconnect(int outputIndex)
        {
            _firstProvider.CurrentOutputIndex = outputIndex;

            if (_firstProvider.Next == null) return;

            IBlock secondProvider = null;
            if (_firstProvider.Next is IBlock block)
                secondProvider = block;

            if (secondProvider != null)
            {
                secondProvider.Facade.transform.SetParent(null);
                secondProvider.Facade.DraggableBlockButton.SetDragUsage(true);
                secondProvider.Facade.Collider.enabled = true;
                secondProvider.Facade.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            }

            _firstProvider.Next = null;
        }
    }
}