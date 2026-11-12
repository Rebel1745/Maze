using UnityEngine;
using MazeGame.Services;
using MazeGame.Data;

namespace MazeGame.Presentation
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Sprite _playerSprite;
        private GameObject _playerGO;
        private MazeCell _currentCell;
        private bool _isPlayerActive = false;

        private IInputService _inputService;
        private IMazeService _mazeService;
        private IPathService _pathService;



        public void Setup(IInputService inputService, IMazeService mazeService, IPathService pathService)
        {
            _inputService = inputService;
            _mazeService = mazeService;
            _pathService = pathService;

            // Subscribe to input events after injection
            _inputService.OnMove += HandleMoveInput;
            _inputService.OnConfirm += HandleConfirmInput;
        }

        private void HandleMoveInput(Vector2 direction)
        {
            if (!_isPlayerActive || _inputService == null) return;

            int targetCellX = _currentCell.X + Mathf.RoundToInt(direction.x);
            int targetCellY = _currentCell.Y + Mathf.RoundToInt(direction.y);

            if (!_mazeService.CellIsValid(targetCellX, targetCellY)) return;

            MazeCell targetCell = _mazeService.GetCellAt(targetCellX, targetCellY);

            // Validate move against Maze Data
            if (CanMoveTo(targetCell))
            {
                MovePlayer(targetCell);
                //_pathService.ExtendPath(_currentCell);

                // Optional: Update Player Visual Position here
                // transform.position = GridUtils.CellToWorld(_currentCell, cellSize);
            }
        }

        private void HandleConfirmInput()
        {
            if (!_isPlayerActive) return;

            // Example: Confirm path or Start a new path
            //_pathService.StartPath(new(_currentCell.X, _currentCell.Y, 0));
        }

        private bool CanMoveTo(MazeCell targetCell)
        {
            if (_mazeService?.CurrentMaze == null) return false;

            // Check bounds
            if (targetCell.X < 0 || targetCell.X >= _mazeService.CurrentMaze.Width ||
                targetCell.Y < 0 || targetCell.Y >= _mazeService.CurrentMaze.Height)
            {
                return false;
            }

            // Determine which wall is between current and target
            if (targetCell.X > _currentCell.X)
                return !_currentCell.HasWall(WallMask.East);
            if (targetCell.X < _currentCell.X)
                return !_currentCell.HasWall(WallMask.West);
            if (targetCell.Y > _currentCell.Y)
                return !_currentCell.HasWall(WallMask.North);
            if (targetCell.Y < _currentCell.Y)
                return !_currentCell.HasWall(WallMask.South);

            return false;
        }

        public void SetupPlayer()
        {
            _currentCell = _mazeService.GetCellAt(0, 0);
            _isPlayerActive = true;

            _playerGO = new GameObject("Player");
            _playerGO.transform.SetParent(transform);
            _playerGO.transform.position = _currentCell.Centre;

            SpriteRenderer sr = _playerGO.AddComponent<SpriteRenderer>();
            sr.sprite = _playerSprite;
            sr.sortingLayerName = "Player";
            sr.sortingOrder = 0;
        }

        private void MovePlayer(MazeCell targetCell)
        {
            _currentCell = targetCell;
            _playerGO.transform.position = targetCell.Centre;
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