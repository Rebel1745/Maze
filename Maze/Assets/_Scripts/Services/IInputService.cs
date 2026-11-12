using UnityEngine;
using System;

namespace MazeGame.Services
{
    public interface IInputService
    {
        // Events
        public event Action<Vector2> OnMove;
        public event Action OnConfirm;
        //event Action OnBacktrack; // Optional: specific action for backtracking

        // Lifecycle
        public void Enable();
        public void Disable();
        public void Dispose(); // Cleanup subscriptions
    }
}