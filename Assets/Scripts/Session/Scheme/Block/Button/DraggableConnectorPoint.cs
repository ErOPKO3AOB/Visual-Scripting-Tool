using Session.Scheme.Connector;
using System.Collections;
using UnityEngine;

namespace Session.Scheme.Block.Button
{
    public class DraggableConnectorPoint : BaseBlockButton
    {
        public void ConstructManually(ActionConnecorFacade connecorFacade)
        {
            _connecorFacade = connecorFacade;
        }

        private ActionConnecorFacade _connecorFacade;

        private Vector3 _worldPointerPosition;
        private bool _dragging;
        private int _currentIndex;

        protected override void Start()
        {
            base.Start();
        }

        public void SetWorldMousePosition(Vector3 position)
        {
            _worldPointerPosition = position;
            _dragging = true;
        }

        public override void Use()
        {
            _currentIndex = _connecorFacade.LineRenderer.positionCount - 1;
            StartCoroutine(UseProccesRoutine());
        }

        private IEnumerator UseProccesRoutine()
        {
            while (_dragging)
            {
                _connecorFacade.LineRenderer.SetPosition(_currentIndex, _worldPointerPosition);
                
                yield return null;
            }
        }

        public void StopUsage()
        {
            _dragging = false;
        }
    }
}