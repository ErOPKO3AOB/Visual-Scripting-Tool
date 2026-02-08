using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Button;
using System;
using UnityEngine;
using UnityEngine.Events;
using VContainer.Unity;

namespace User
{
    public class WorldUIControllerService : IInitializable, ILateTickable, IDisposable
    {
        #region Initialization
        public WorldUIControllerService(InputService inputService, CameraControllerFacade cameraControllerFacade, BlockConfigs blockConfigs)
        {
            _inputService = inputService;
            _camera = cameraControllerFacade.Camera;
            _blockConfigs = blockConfigs;
        }

        private readonly InputService _inputService;
        private readonly Camera _camera;
        private readonly BlockConfigs _blockConfigs;

        public UnityAction<BaseBlockButton> OnInteract;
        public UnityAction<BaseBlockButton> OnStopInteract;

        private BaseBlockButton _currentObject;
        private Vector2 _lastPointerPosition;

        public void Initialize()
        {
            _inputService.OnClick += OnClick;
            _inputService.OnPointerPosition += OnPointerPosition;
        }
        #endregion

        #region Checks
        public void LateTick()
        {
            InteractionProcess();
        }

        private void OnClick(bool isClicked)
        {
            if (isClicked)
            {
                TryStartIntercation();
            }

            else
            {
                StopInteraction();
            }
        }

        private void OnPointerPosition(Vector2 pointerPosition)
        {
            _lastPointerPosition = pointerPosition;
        }

        private void TryStartIntercation()
        {
            Vector2 worldPoint = _camera.ScreenToWorldPoint(_lastPointerPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 1);

            if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out _currentObject))
            {
                _currentObject.Use();
                OnInteract?.Invoke(_currentObject);
            }
        }
        #endregion

        #region Interaction
        private void InteractionProcess()
        {
            if (_currentObject == null) return;

            if (_currentObject is DraggableBlockButton draggableBlockButton)
            {
                draggableBlockButton.SetWorldMousePosition(_camera.ScreenToWorldPoint(_lastPointerPosition));
            }

            else if (_currentObject is DraggableConnectorPoint draggableConnectorPoint)
            {
                draggableConnectorPoint.SetWorldMousePosition(_camera.ScreenToWorldPoint(_lastPointerPosition));
            }
        }

        private void StopInteraction()
        {
            if (_currentObject != null)
            {
                if (_currentObject is DraggableBlockButton draggableBlockButton)
                    draggableBlockButton.StopUsage();
                else if (_currentObject is BlockOutputButton blockOutputButton)
                    blockOutputButton.StopUsage();
                else if (_currentObject is BlockInputTrigger blockInputTrigger)
                    blockInputTrigger.StopUsage();

                OnStopInteract?.Invoke(_currentObject);

                _currentObject = null;
            }
        }

        public void Dispose()
        {
            _inputService.OnClick -= OnClick;
            _inputService.OnPointerPosition -= OnPointerPosition;
        }
        #endregion
    }
}