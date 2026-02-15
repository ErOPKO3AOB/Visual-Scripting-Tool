using Session.Scheme.Connector;
using System.Collections;
using UnityEngine;

namespace Session.Scheme.Block.Button
{
    public class DraggableConnectorPoint : BaseBlockButton
    {
        public void ConstructManually(ActionConnecorFacade connecorFacade, IBlock block, BlockOutputButton blockOutputButton)
        {
            _connecorFacade = connecorFacade;
            _block = block;
            _blockOutputButton = blockOutputButton;
        }

        private ActionConnecorFacade _connecorFacade;
        private IBlock _block;
        private BlockOutputButton _blockOutputButton;

        private Vector3 _localPointerPosition;
        private bool _dragging;
        private int _currentIndex;

        private Vector3 _dragStartLocalPosition;
        private Vector3 _wireDirection;

        public ActionConnecorFacade ConnecorFacade => _connecorFacade;
        public IBlock Block => _block;
        public BlockOutputButton BlockOutputButton => _blockOutputButton;

        protected override void Start()
        {
            base.Start();

            int lastIndex = _connecorFacade.LineRenderer.positionCount - 1;
            Vector3 lastPointLocal = _connecorFacade.LineRenderer.GetPosition(lastIndex);
            transform.localPosition = lastPointLocal;

            UpdateWireDirection();
        }

        private void UpdateWireDirection()
        {
            int pointCount = _connecorFacade.LineRenderer.positionCount;
            if (pointCount >= 2)
            {
                Vector3 currentPoint = transform.localPosition;
                Vector3 previousPoint = _connecorFacade.LineRenderer.GetPosition(pointCount - 2);
                _wireDirection = (currentPoint - previousPoint).normalized;
            }
            else
            {
                _wireDirection = Vector3.right;
            }
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
            _dragStartLocalPosition = transform.localPosition;
            UpdateWireDirection();

            _connecorFacade.LineRenderer.positionCount += 1;
            _currentIndex = _connecorFacade.LineRenderer.positionCount - 1;

            _dragging = true;

            while (_dragging)
            {
                Vector3 mouseDelta = _localPointerPosition - _dragStartLocalPosition;

                Vector3 newLocalPosition = _dragStartLocalPosition;

                if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
                {
                    if (Mathf.Abs(_wireDirection.x) > 0.1f)
                    {
                        if ((_wireDirection.x > 0 && mouseDelta.x >= 0) ||
                            (_wireDirection.x < 0 && mouseDelta.x <= 0))
                        {
                            newLocalPosition.x = _dragStartLocalPosition.x + mouseDelta.x;
                        }
                    }
                    else
                    {
                        newLocalPosition.x = _dragStartLocalPosition.x + mouseDelta.x;
                    }
                }
                else
                {
                    if (Mathf.Abs(_wireDirection.y) > 0.1f)
                    {
                        if ((_wireDirection.y > 0 && mouseDelta.y >= 0) ||
                            (_wireDirection.y < 0 && mouseDelta.y <= 0))
                        {
                            newLocalPosition.y = _dragStartLocalPosition.y + mouseDelta.y;
                        }
                    }
                    else
                    {
                        newLocalPosition.y = _dragStartLocalPosition.y + mouseDelta.y;
                    }
                }

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