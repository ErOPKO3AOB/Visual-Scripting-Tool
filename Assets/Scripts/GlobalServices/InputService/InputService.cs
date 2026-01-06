using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using VContainer.Unity;
using InputSettings = GlobalServices.ProjectLifetime.InputSettings;

namespace GlobalServices.Input
{
    public class InputService : IInitializable
    {
        public InputService(InputSettings settings, PlayerInput playerInput)
        {
            _settings = settings;
            _playerInput = playerInput;
        }

        private readonly InputSettings _settings;
        private readonly PlayerInput _playerInput;

        public UnityAction<Vector2> OnZoom;
        public UnityAction<Vector2> OnDragDelta;
        public UnityAction<bool> OnClick;
        public UnityAction<Vector2> OnPointerPosition;

        public void Initialize()
        {
            _playerInput.actions["Zoom"].started += ZoomInput;
            _playerInput.actions["Zoom"].performed += ZoomInput;
            _playerInput.actions["Zoom"].canceled += ZoomInput;
            _playerInput.actions["Drag"].started += DragInput;
            _playerInput.actions["Drag"].canceled += DragInput;

            _playerInput.actions["Click"].started += TouchInput;
            _playerInput.actions["Click"].performed += TouchInput;
            _playerInput.actions["Click"].canceled += TouchInput;

            _playerInput.actions["PointerPosition"].started += PointerPositionInput;
            _playerInput.actions["PointerPosition"].performed += PointerPositionInput;
            _playerInput.actions["PointerPosition"].canceled += PointerPositionInput;
        }

        private void ZoomInput(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnZoom?.Invoke(context.ReadValue<Vector2>());
        }

        private void DragInput(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
                OnDragDelta?.Invoke(context.ReadValue<Vector2>());
        }

        private void TouchInput(InputAction.CallbackContext context)
        {
            OnClick?.Invoke(context.performed && !context.canceled);
        }

        private void PointerPositionInput(InputAction.CallbackContext context)
        {
            OnPointerPosition?.Invoke(context.ReadValue<Vector2>());
        }
    }
}