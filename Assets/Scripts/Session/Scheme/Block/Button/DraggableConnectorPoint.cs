using Session.Scheme.Connector;
using System.Collections;
using UnityEngine;

namespace Session.Scheme.Block.Button
{
    public class DraggableConnectorPoint : BaseBlockButton
    {
        public void ConstructManually(ActionConnecorFacade connecorFacade, IBlock block)
        {
            _connecorFacade = connecorFacade;
            _block = block;
        }

        private ActionConnecorFacade _connecorFacade;
        private IBlock _block;
        public IBlock Block => _block;

        public ActionConnecorFacade ConnecorFacade => _connecorFacade;

        private Vector3 _localPointerPosition;
        private bool _dragging;
        private int _currentIndex;

        private Vector3 _dragStartLocalPosition;
        private bool _isXAxisMovement = false;

        protected override void Start()
        {
            base.Start();

            int lastIndex = _connecorFacade.LineRenderer.positionCount - 1;
            Vector3 lastPointLocal = _connecorFacade.LineRenderer.GetPosition(lastIndex);
            transform.localPosition = lastPointLocal;
        }

        public void SetWorldMousePosition(Vector3 worldPosition)
        {
            _localPointerPosition = _connecorFacade.transform.InverseTransformPoint(worldPosition);
        }

        public override void Use()
        {
            StartCoroutine(UseProccesRoutine());
        }

        private IEnumerator UseProccesRoutine()
        {
            float timer = 0f;
            float waitTime = 0.2f;

            _dragStartLocalPosition = transform.localPosition;

            while (timer < waitTime)
            {
                Vector3 mouseDelta = _localPointerPosition - _dragStartLocalPosition;

                if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
                    _isXAxisMovement = true;
                else
                    _isXAxisMovement = false;

                timer += Time.deltaTime;
                yield return null;
            }

            _connecorFacade.LineRenderer.positionCount += 1;
            _currentIndex = _connecorFacade.LineRenderer.positionCount - 1;

            _dragging = true;

            while (_dragging)
            {
                Vector3 mouseDelta = _localPointerPosition - _dragStartLocalPosition;
                Vector3 constrainedDelta = Vector3.zero;

                if (_isXAxisMovement)
                    constrainedDelta = new Vector3(mouseDelta.x, 0f, 0f);
                else
                    constrainedDelta = new Vector3(0f, mouseDelta.y, 0f);

                Vector3 newLocalPosition = _dragStartLocalPosition + constrainedDelta;

                transform.localPosition = newLocalPosition;

                _connecorFacade.LineRenderer.SetPosition(_currentIndex, newLocalPosition);

                yield return null;
            }
        }

        public void StopUsage()
        {
            _dragging = false;
        }
    }
}