using GlobalServices.ProjectLifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using User.Input;
using VContainer.Unity;

namespace User
{
    public class DraggableObjectController : IInitializable, ITickable, IDisposable
    {
        public DraggableObjectController(InputService inputService, CameraControllerFacade cameraControllerFacade, BlockConfigs blockConfigs)
        {
            _inputService = inputService;
            _camera = cameraControllerFacade.Camera;
            _blockConfigs = blockConfigs;
        }

        private readonly InputService _inputService;
        private readonly Camera _camera;
        private readonly BlockConfigs _blockConfigs;

        private Vector3 _dragOffset;
        private Vector2 _lastPointerPosition;

        private GameObject _currentDraggedObject;
        private SpriteRenderer _currentSpriteRenderer;
        private Color _objectStartColor;
        private Vector3 _objectStartScale;

        // Настройки
        private const float DRAG_SPEED = 10f;

        public UnityAction OnDrag;
        public UnityAction OnStopDrag;

        public void Initialize()
        {
            // Подписываемся на события ввода
            _inputService.OnClick += OnClick;
            _inputService.OnPointerPosition += OnPointerPosition;
        }

        public void Tick()
        {
            if (_currentDraggedObject != null)
            {
                UpdateDraggedObjectPosition();
            }
        }

        private void OnClick(bool isClicked)
        {
            if (isClicked)
            {
                TryStartDrag();
            }
            else
            {
                StopDrag();
            }
        }

        private void OnPointerPosition(Vector2 pointerPosition)
        {
            _lastPointerPosition = pointerPosition;
        }

        private void TryStartDrag()
        {
            // Проверяем Raycast для 2D объектов
            Vector2 worldPoint = _camera.ScreenToWorldPoint(_lastPointerPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                _currentDraggedObject = hit.collider.gameObject;
                _currentSpriteRenderer = _currentDraggedObject.GetComponent<SpriteRenderer>();
                _objectStartColor = _currentSpriteRenderer.color;
                _objectStartScale = _currentDraggedObject.transform.localScale;

                if (_currentDraggedObject != null)
                {
                    // Начинаем перетаскивание
                    if (_currentSpriteRenderer != null)
                    {
                        _currentSpriteRenderer.color = _blockConfigs.DraggingColorAffect;
                    }

                    // Легкое увеличение при захвате
                    _currentDraggedObject.transform.localScale = _objectStartScale * _blockConfigs.DraggingSizeAffect;

                    // Рассчитываем смещение
                    Vector3 hitPoint = hit.point;
                    _dragOffset = _currentDraggedObject.transform.position - new Vector3(hitPoint.x, hitPoint.y, _currentDraggedObject.transform.position.z);

                    OnDrag?.Invoke();
                }
            }
        }

        private void UpdateDraggedObjectPosition()
        {
            if (_currentDraggedObject == null) return;

            Vector2 worldPoint = _camera.ScreenToWorldPoint(_lastPointerPosition);
            Vector3 targetPosition = new Vector3(worldPoint.x, worldPoint.y, _currentDraggedObject.transform.position.z) + _dragOffset;

            // Плавное перемещение
            _currentDraggedObject.transform.position = Vector3.Lerp(
                _currentDraggedObject.transform.position,
                targetPosition,
                Time.deltaTime * DRAG_SPEED
            );
        }

        private void StopDrag()
        {
            if (_currentDraggedObject != null)
            {
                // Восстанавливаем внешний вид
                if (_currentSpriteRenderer != null)
                {
                    _currentSpriteRenderer.color = _objectStartColor;
                }

                _currentDraggedObject.transform.localScale = _objectStartScale;
                _currentDraggedObject = null;

                OnStopDrag?.Invoke();
            }
        }

        public void Dispose()
        {
            _inputService.OnClick -= OnClick;
            _inputService.OnPointerPosition -= OnPointerPosition;
        }
    }
}