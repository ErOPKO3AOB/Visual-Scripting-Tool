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

        // ƒл€ фиксации начальной точки при перетаскивании
        private Vector3 _dragStartCameraPosition;
        private Vector2 _dragStartPointerPosition;

        // ƒл€ реализации IInterruptable
        private bool _isInterrupted = false;
        private Vector3 _positionBeforeInterrupt;
        private float _zoomBeforeInterrupt;
        private bool _wasDraggingBeforeInterrupt;
        
        private Vector2 _lastPointerPosition;

        public void Initialize()
        {
            _targetPosition = _facade.transform.position;
            _targetZoom = _facade.Camera.orthographicSize;

            _inputService.OnClick += OnClick;
            _inputService.OnDragDelta += HandleDragDelta;
            _inputService.OnZoom += HandleMouseZoom;
            _inputService.OnPointerPosition += OnPointerPosition;
        }

        public void Tick()
        {
            if (_isInterrupted) return;

            //  онтролируема€ плавность перемещени€ камеры
            _facade.transform.position = Vector3.SmoothDamp(
                _facade.transform.position,
                _targetPosition,
                ref _velocity,
                _settings.MoveSmoothTime
            );

            // ѕлавный зум
            _facade.Camera.orthographicSize = Mathf.SmoothDamp(
                _facade.Camera.orthographicSize,
                _targetZoom,
                ref _zoomVelocity,
                _settings.ZoomSmoothTime
            );
        }

        private void OnClick(bool click)
        {
            if (_isInterrupted) return;

            _canDragCamera = click;

            if (click)
            {
                // «апоминаем начальную позицию камеры и курсора при начале перетаскивани€
                _dragStartCameraPosition = _facade.transform.position;
                _dragStartPointerPosition = _lastPointerPosition;
                _velocity = Vector3.zero; // —брасываем скорость при начале нового перетаскивани€
            }
        }

        private void OnPointerPosition(Vector2 pointerPosition)
        {
            _lastPointerPosition = pointerPosition;
        }

        private void HandleDragDelta(Vector2 delta)
        {
            if (_isInterrupted) return;

            if (!_canDragCamera || delta == Vector2.zero) return;

            // ѕреобразуем начальную и текущую позиции курсора в мировые координаты
            Vector3 startWorldPos = _facade.Camera.ScreenToWorldPoint(new Vector3(
                _dragStartPointerPosition.x,
                _dragStartPointerPosition.y,
                -_facade.Camera.transform.position.z
            ));

            Vector3 currentWorldPos = _facade.Camera.ScreenToWorldPoint(new Vector3(
                _lastPointerPosition.x,
                _lastPointerPosition.y,
                -_facade.Camera.transform.position.z
            ));

            // ¬ычисл€ем смещение в мировых координатах
            Vector3 worldOffset = startWorldPos - currentWorldPos;

            // Ќова€ целева€ позици€ = начальна€ позици€ камеры + смещение
            // Ёто гарантирует, что точка, за которую ухватились, останетс€ под курсором
            _targetPosition = _dragStartCameraPosition + worldOffset;
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

        // –еализаци€ IInterruptable
        public void Interrupt()
        {
            if (_isInterrupted) return;

            _isInterrupted = true;
            _wasDraggingBeforeInterrupt = _canDragCamera;
            _canDragCamera = false;

            // —охран€ем текущие значени€ дл€ возможного восстановлени€
            _positionBeforeInterrupt = _targetPosition;
            _zoomBeforeInterrupt = _targetZoom;

            // —брасываем скорости дл€ плавного останова
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
            _inputService.OnPointerPosition -= OnPointerPosition;

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