using UnityEngine;
using UnityEngine.InputSystem; // Requires Unity Input System package
using System;

namespace MazeGame.Services
{
    public class InputService : IInputService
    {
        private PlayerInputActions _inputActions;

        public event Action<Vector2> OnMove;
        public event Action OnConfirm;
        //public event Action OnBacktrack;

        public InputService()
        {
            _inputActions = new PlayerInputActions();
            
            // Subscribe to the New Input System callbacks
            _inputActions.Player.Move.performed += OnMovePerformed;
            _inputActions.Player.Move.canceled += OnMoveCanceled; // Optional: handle zero movement
            
            _inputActions.Player.Confirm.performed += OnConfirmPerformed;
            
            // Example: If you have a specific "Backtrack" button
            // _inputActions.Player.Backtrack.performed += OnBacktrackPerformed;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            // Optional: Normalize diagonal inputs or snap to grid directions
            // For a maze, you might want to snap to cardinal directions
            if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
                value = new Vector2(Mathf.Sign(value.x), 0);
            else
                value = new Vector2(0, Mathf.Sign(value.y));

            OnMove?.Invoke(value);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            // Optional: If you want to detect "stop" or reset velocity
        }

        private void OnConfirmPerformed(InputAction.CallbackContext context)
        {
            OnConfirm?.Invoke();
        }

        // private void OnBacktrackPerformed(InputAction.CallbackContext context)
        // {
        //     OnBacktrack?.Invoke();
        // }

        public void Enable() => _inputActions.Enable();
        public void Disable() => _inputActions.Disable();
        
        public void Dispose()
        {
            _inputActions.Dispose();
        }
    }
}