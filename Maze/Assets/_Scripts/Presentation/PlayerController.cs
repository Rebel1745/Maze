using UnityEngine;
using MazeGame.Services;
using MazeGame.Data;

namespace MazeGame.Presentation
{
    public class PlayerController : MonoBehaviour
    {
        private IInputService _inputService;
        private IMazeService _mazeService;
        private IPathService _pathService;

        private Vector3Int _currentCell;
        private bool _isPlayerActive = false;

        /// <summary>
        /// Called by GameController to inject dependencies.
        /// Must be called before any other method on this component.
        /// </summary>
        public void Setup(IInputService inputService, IMazeService mazeService, IPathService pathService)
        {
            _inputService = inputService;
            _mazeService = mazeService;
            _pathService = pathService;

            // Subscribe to input events after injection
            _inputService.OnMove += HandleMoveInput;
            _inputService.OnConfirm += HandleConfirmInput;
        }

        private void Start()
        {
            // Initialize player position (e.g., top-left corner)
            _currentCell = new Vector3Int(0, 0, 0);
            _isPlayerActive = true;
        }

        private void HandleMoveInput(Vector2 direction)
        {
            if (!_isPlayerActive || _inputService == null) return;

            Vector3Int targetCell = _currentCell;

            // Apply direction to current cell
            targetCell.x += Mathf.RoundToInt(direction.x);
            targetCell.y += Mathf.RoundToInt(direction.y);

            // Validate move against Maze Data
            if (CanMoveTo(targetCell))
            {
                _currentCell = targetCell;
                _pathService.ExtendPath(_currentCell);

                // Optional: Update Player Visual Position here
                // transform.position = GridUtils.CellToWorld(_currentCell, cellSize);
            }
        }

        private void HandleConfirmInput()
        {
            if (!_isPlayerActive) return;

            // Example: Confirm path or Start a new path
            _pathService.StartPath(_currentCell);
        }

        private bool CanMoveTo(Vector3Int targetCell)
        {
            if (_mazeService?.CurrentMaze == null) return false;

            // Check bounds
            if (targetCell.x < 0 || targetCell.x >= _mazeService.CurrentMaze.Width ||
                targetCell.y < 0 || targetCell.y >= _mazeService.CurrentMaze.Height)
            {
                return false;
            }

            // Check walls using bitmask
            MazeCell currentCell = _mazeService.GetCellAt(_currentCell.x, _currentCell.y);

            // Determine which wall is between current and target
            if (targetCell.x > _currentCell.x)
                return !currentCell.HasWall(WallMask.East);
            if (targetCell.x < _currentCell.x)
                return !currentCell.HasWall(WallMask.West);
            if (targetCell.y > _currentCell.y)
                return !currentCell.HasWall(WallMask.North);
            if (targetCell.y < _currentCell.y)
                return !currentCell.HasWall(WallMask.South);

            return false;
        }

        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks
            if (_inputService != null)
            {
                _inputService.OnMove -= HandleMoveInput;
                _inputService.OnConfirm -= HandleConfirmInput;
            }
        }
    }
}