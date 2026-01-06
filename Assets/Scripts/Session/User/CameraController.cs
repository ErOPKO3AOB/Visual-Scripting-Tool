using GlobalServices.Input;
using GlobalServices.ProjectLifetime;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Session.User
{
    public class CameraController : IInitializable, ITickable, IDisposable
    {
        [Inject]
        public CameraController(CameraControllerFacade facade, CameraSettings settings, InputService inputService)
        {
            _facade = facade;
            _settings = settings;
            _inputService = inputService;
        }

        private readonly CameraControllerFacade _facade;
        private readonly CameraSettings _settings;
        private readonly InputService _inputService;

        private bool _canDragCamera;
        private bool _draggingCube = false;
        private Vector3 _targetPosition;
        private float _targetZoom;
        private Vector3 _velocity = Vector3.zero;
        private float _zoomVelocity;

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
            if (_canDragCamera && !_draggingCube)
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
            _canDragCamera = click;
        }

        private void HandleDragDelta(Vector2 delta)
        {
            // Проверяем, что клик активен и есть движение
            if (!_canDragCamera || delta == Vector2.zero) return;

            // Получаем текущий зум (orthographicSize)
            float currentZoom = _facade.Camera.orthographicSize;

            // Коэффициент зависимости чувствительности от зума
            // Чем больше зум (ближе камера) - тем меньше чувствительность
            float zoomFactor = Mathf.Clamp(currentZoom / 15f, 0.3f, 3f);

            // Применяем delta с учетом текущего зума
            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * _settings.MoveSensitivity * 0.01f * zoomFactor;
            _targetPosition += move;
        }

        private void HandleMouseZoom(Vector2 zoomInput)
        {
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

        public void Dispose()
        {
            _inputService.OnDragDelta -= HandleDragDelta;
            _inputService.OnClick -= OnClick;
            _inputService.OnZoom -= HandleMouseZoom;
        }
    }
}