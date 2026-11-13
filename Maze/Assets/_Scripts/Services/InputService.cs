using UnityEngine;
using UnityEngine.InputSystem; // Requires Unity Input System package
using System;

namespace MazeGame.Services
{
    public class InputService : MonoBehaviour, IInputService
    {
        private PlayerInputActions _inputActions;

        public event Action<Vector2> OnMove;
        public event Action OnConfirm;

        private bool _isMoving = false;
        private Vector2 _moveValue;

        private void Awake()
        {
            _inputActions = new PlayerInputActions();

            // Subscribe to the New Input System callbacks
            _inputActions.Player.Move.performed += OnMovePerformed;
            _inputActions.Player.Move.canceled += OnMoveCancelled;

            _inputActions.Player.Confirm.performed += OnConfirmPerformed;
        }

        private void Update()
        {
            if (_isMoving) OnMove?.Invoke(_moveValue);
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            _moveValue = context.ReadValue<Vector2>();

            if (_moveValue == Vector2.zero)
            {
                _isMoving = false;
                return;
            }

            if (Mathf.Abs(_moveValue.x) > Mathf.Abs(_moveValue.y))
                _moveValue = new Vector2(Mathf.Sign(_moveValue.x), 0);
            else
                _moveValue = new Vector2(0, Mathf.Sign(_moveValue.y));

            _isMoving = true;
        }

        private void OnMoveCancelled(InputAction.CallbackContext context)
        {
            _isMoving = false;
        }

        private void OnConfirmPerformed(InputAction.CallbackContext context)
        {
            OnConfirm?.Invoke();
        }

        public void Enable() => _inputActions.Enable();
        public void Disable() => _inputActions.Disable();

        public void Dispose()
        {
            _inputActions.Dispose();
        }
    }
}