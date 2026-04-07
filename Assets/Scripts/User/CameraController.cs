using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Button;
using Session.Scheme.Windows;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace User
{
    public class CameraController : IInitializable, ITickable, IDisposable, IPostInitializable
    {
        [Inject]
        public CameraController(CameraControllerFacade facade, CameraSettings settings, InputService inputService, WorldUIControllerService worldUIControllerService, WindowFactory windowService)
        {
            _facade = facade;
            _settings = settings;
            _inputService = inputService;
            _worldUIControllerService = worldUIControllerService;
            _windowService = windowService;
        }

#if UNITY_ANDROID
        private bool _dragPotential;
        private Vector2 _dragPotentialStart;
#endif

        private readonly CameraControllerFacade _facade;
        private readonly CameraSettings _settings;
        private readonly InputService _inputService;
        private readonly WorldUIControllerService _worldUIControllerService;
        private readonly WindowFactory _windowService;

        private bool _canDragCamera;
        private Vector3 _targetPosition;
        private float _targetZoom;
        private Vector3 _velocity = Vector3.zero;
        private float _zoomVelocity;

        // Для фиксации начальной точки при перетаскивании
        private Vector3 _dragStartCameraPosition;
        private Vector2 _dragStartPointerPosition;

        // Для реализации IInterruptable
        private bool _isInterrupted = false;
        private Vector3 _positionBeforeInterrupt;
        private float _zoomBeforeInterrupt;
        private bool _wasDraggingBeforeInterrupt;

        private Vector2 _lastPointerPosition;

        public float MoveSensitivityMultiplier { get; set; } = 1.0f;
        public float ZoomSensitivityMultiplier { get; set; } = 1.0f;

        public bool CanDragCamera { get { return _canDragCamera; } set { _canDragCamera = value; } }

        public void Initialize()
        {
            _targetPosition = _facade.transform.position;
            _targetZoom = _facade.Camera.orthographicSize;

            _inputService.OnClick += OnClick;
            _inputService.OnDragDelta += HandleDragDelta;
            _inputService.OnZoom += HandleMouseZoom;
            _inputService.OnPointerPosition += OnPointerPosition;

            _worldUIControllerService.CameraController = this;
        }

        public void Tick()
        {
            if (_isInterrupted) return;

            _facade.transform.position = Vector3.SmoothDamp(
                _facade.transform.position,
                _targetPosition,
                ref _velocity,
                _settings.MoveSmoothTime / MoveSensitivityMultiplier
            );

            _facade.Camera.orthographicSize = Mathf.SmoothDamp(
                _facade.Camera.orthographicSize,
                _targetZoom,
                ref _zoomVelocity,
                _settings.ZoomSmoothTime / ZoomSensitivityMultiplier
            );
        }

        private void OnClick(bool click)
        {
            if (_isInterrupted) return;

#if UNITY_ANDROID
            if (click)
            {
                _dragPotential = true;
                _dragPotentialStart = _lastPointerPosition;
                _dragStartPointerPosition = _lastPointerPosition;
                _dragStartCameraPosition = _facade.transform.position;
                _velocity = Vector3.zero;
            }
            else
            {
                _dragPotential = false;
                _canDragCamera = false;
            }
#else
    _canDragCamera = click;
    if (click)
    {
        _dragStartCameraPosition = _facade.transform.position;
        _dragStartPointerPosition = _lastPointerPosition;
        _velocity = Vector3.zero;
    }
#endif
        }

        private void OnPointerPosition(Vector2 pointerPosition)
        {
            _lastPointerPosition = pointerPosition;
        }

        private void HandleDragDelta(Vector2 delta)
        {
            if (_isInterrupted) return;
            if (delta == Vector2.zero) return;

#if UNITY_ANDROID
            if (_canDragCamera)
            {
                Vector3 startWorldPos = _facade.Camera.ScreenToWorldPoint(new Vector3(
                    _dragStartPointerPosition.x, _dragStartPointerPosition.y, -_facade.Camera.transform.position.z));
                Vector3 currentWorldPos = _facade.Camera.ScreenToWorldPoint(new Vector3(
                    _lastPointerPosition.x, _lastPointerPosition.y, -_facade.Camera.transform.position.z));
                Vector3 worldOffset = startWorldPos - currentWorldPos;
                _targetPosition = _dragStartCameraPosition + worldOffset;
                return;
            }

            if (_dragPotential)
            {
                float distSqr = (_lastPointerPosition - _dragPotentialStart).sqrMagnitude;
                float threshold = _settings.MobileDragThresholdPixels;
                if (distSqr >= threshold * threshold)
                {
                    _canDragCamera = true;
                    _dragStartPointerPosition = _lastPointerPosition;
                    _dragStartCameraPosition = _facade.transform.position;
                    _velocity = Vector3.zero;
                    _dragPotential = false;
                }
            }
#else
    if (!_canDragCamera) return;

    Vector3 startWorldPos = _facade.Camera.ScreenToWorldPoint(new Vector3(
        _dragStartPointerPosition.x, _dragStartPointerPosition.y, -_facade.Camera.transform.position.z));
    Vector3 currentWorldPos = _facade.Camera.ScreenToWorldPoint(new Vector3(
        _lastPointerPosition.x, _lastPointerPosition.y, -_facade.Camera.transform.position.z));
    Vector3 worldOffset = startWorldPos - currentWorldPos;
    _targetPosition = _dragStartCameraPosition + worldOffset;
#endif
        }

        private void HandleMouseZoom(Vector2 zoomInput)
        {
            if (_isInterrupted) return;

            if (zoomInput == Vector2.zero) return;

            float zoomDelta;
            float currentZoom = _facade.Camera.orthographicSize;

            if (Application.isMobilePlatform)
            {
                zoomDelta = zoomInput.x * 0.1f * _settings.ZoomSensitivity;
            }
            else
            {
                zoomDelta = zoomInput.y * _settings.ZoomSensitivity;
            }

            _targetZoom -= zoomDelta;
            _targetZoom = Mathf.Clamp(_targetZoom, 1f, 30f);
        }

        public void Interrupt(BaseBlockButton baseBlockButton)
        {
            if (_isInterrupted) return;
            _isInterrupted = true;
            _wasDraggingBeforeInterrupt = _canDragCamera;
            _canDragCamera = false;
#if UNITY_ANDROID
            _dragPotential = false;
#endif
            _positionBeforeInterrupt = _targetPosition;
            _zoomBeforeInterrupt = _targetZoom;
            _velocity = Vector3.zero;
            _zoomVelocity = 0f;
        }

        public void StopInterruption(BaseBlockButton baseBlockButton)
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

            _worldUIControllerService.OnInteractCallback -= Interrupt;
            _worldUIControllerService.OnStopInteractCallback -= StopInterruption;
        }

        public void PostInitialize()
        {
            _worldUIControllerService.OnInteractCallback += Interrupt;
            _worldUIControllerService.OnStopInteractCallback += StopInterruption;
        }
    }
}