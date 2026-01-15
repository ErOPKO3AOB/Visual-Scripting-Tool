using GlobalServices.ProjectLifetime;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using User.Input;

namespace User
{
    public class CameraController : IInitializable, ITickable, IDisposable, IPostInitializable
    {
        [Inject]
        public CameraController(CameraControllerFacade facade, CameraSettings settings, InputService inputService, DraggableObjectController draggableObjectController)
        {
            _facade = facade;
            _settings = settings;
            _inputService = inputService;
            _draggableObjectController = draggableObjectController;
        }

        private readonly CameraControllerFacade _facade;
        private readonly CameraSettings _settings;
        private readonly InputService _inputService;
        private readonly DraggableObjectController _draggableObjectController;

        private bool _canDragCamera;
        private Vector3 _targetPosition;
        private float _targetZoom;
        private Vector3 _velocity = Vector3.zero;
        private float _zoomVelocity;

        // Для реализации IInterruptable
        private bool _isInterrupted = false;
        private Vector3 _positionBeforeInterrupt;
        private float _zoomBeforeInterrupt;
        private bool _wasDraggingBeforeInterrupt;

        public void Initialize()
        {
            _targetPosition = _facade.transform.position;
            _targetZoom = _facade.Camera.orthographicSize;

            _inputService.OnClick += OnClick;
            _inputService.OnDragDelta += HandleDragDelta;
            _inputService.OnZoom += HandleMouseZoom;
        }

        public void Tick()
        {
            if (_isInterrupted) return;

            if (_canDragCamera)
            {
                // Плавное перемещение
                _facade.transform.position = Vector3.SmoothDamp(
                    _facade.transform.position,
                    _targetPosition,
                    ref _velocity,
                    0.1f
                );
            }

            // Плавный зум
            _facade.Camera.orthographicSize = Mathf.SmoothDamp(
                _facade.Camera.orthographicSize,
                _targetZoom,
                ref _zoomVelocity,
                0.1f
            );
        }

        private void OnClick(bool click)
        {
            if (_isInterrupted) return;
            _canDragCamera = click;
        }

        private void HandleDragDelta(Vector2 delta)
        {
            if (_isInterrupted) return;

            if (!_canDragCamera || delta == Vector2.zero) return;

            float currentZoom = _facade.Camera.orthographicSize;
            float zoomFactor = Mathf.Clamp(currentZoom / 15f, 0.3f, 3f);

            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * _settings.MoveSensitivity * 0.01f * zoomFactor;
            _targetPosition += move;
        }

        private void HandleMouseZoom(Vector2 zoomInput)
        {
            if (_isInterrupted) return;

            if (zoomInput == Vector2.zero) return;

            float zoomDelta;
            float currentZoom = _facade.Camera.orthographicSize;

            if (Application.isMobilePlatform)
            {
                zoomDelta = zoomInput.x * _settings.ZoomSensitivity * 0.1f;
            }
            else
            {
                zoomDelta = zoomInput.y * _settings.ZoomSensitivity;
            }

            _targetZoom -= zoomDelta;
            _targetZoom = Mathf.Clamp(_targetZoom, 1f, 30f);
        }

        // Реализация IInterruptable
        public void Interrupt()
        {
            if (_isInterrupted) return;

            _isInterrupted = true;
            _wasDraggingBeforeInterrupt = _canDragCamera;
            _canDragCamera = false;

            // Сохраняем текущие значения для возможного восстановления
            _positionBeforeInterrupt = _targetPosition;
            _zoomBeforeInterrupt = _targetZoom;

            // Сбрасываем скорости для плавного останова
            _velocity = Vector3.zero;
            _zoomVelocity = 0f;
        }

        public void StopInterruption()
        {
            if (!_isInterrupted) return;

            _isInterrupted = false;
            _canDragCamera = _wasDraggingBeforeInterrupt;
        }

        public void Dispose()
        {
            _inputService.OnDragDelta -= HandleDragDelta;
            _inputService.OnClick -= OnClick;
            _inputService.OnZoom -= HandleMouseZoom;

            _draggableObjectController.OnDrag -= Interrupt;
            _draggableObjectController.OnStopDrag -= StopInterruption;
        }

        public void PostInitialize()
        {
            _draggableObjectController.OnDrag += Interrupt;
            _draggableObjectController.OnStopDrag += StopInterruption;
        }
    }
}