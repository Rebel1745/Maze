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
        private bool _isPlayerMoving = false;
        private MazeCell _targetCell;
        [SerializeField] private float _moveSpeed = 0.1f;

        private IMazeService _mazeService;
        private IPathService _pathService;

        private void Update()
        {
            if (_isPlayerMoving) MovePlayer();
        }

        public void Setup(IMazeService mazeService, IPathService pathService)
        {
            _mazeService = mazeService;
            _pathService = pathService;

            // Subscribe to input events after injection
            InputManager.Instance.OnMove += HandleMoveInput;
            InputManager.Instance.OnConfirm += HandleConfirmInput;
        }

        private void HandleMoveInput(Vector2 direction)
        {
            if (!_isPlayerActive) return;
            if (direction == Vector2.zero) return;
            if (_isPlayerMoving) return;

            int targetCellX = _currentCell.X + Mathf.RoundToInt(direction.x);
            int targetCellY = _currentCell.Y + Mathf.RoundToInt(direction.y);

            if (!_mazeService.CellIsValid(targetCellX, targetCellY)) return;

            MazeCell targetCell = _mazeService.GetCellAt(targetCellX, targetCellY);

            // Validate move against Maze Data
            if (CanMoveTo(targetCell)) StartMovement(targetCell);
        }

        // Currently Unused
        private void HandleConfirmInput()
        {
            if (!_isPlayerActive) return;
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

            _pathService.ExtendPath(_currentCell);
        }

        private void StartMovement(MazeCell targetCell)
        {
            _isPlayerMoving = true;
            _targetCell = targetCell;
            _pathService.UpdatePath(_playerGO.transform.position, true);
        }

        private void MovePlayer()
        {
            _playerGO.transform.position = Vector3.MoveTowards(_playerGO.transform.position, _targetCell.Centre, _moveSpeed);
            _pathService.UpdatePath(_playerGO.transform.position, false);

            if (Vector3.Distance(_playerGO.transform.position, _targetCell.Centre) < 0.01f) EndPlayerMovement();
        }

        private void EndPlayerMovement()
        {
            _playerGO.transform.position = _targetCell.Centre;
            _currentCell = _targetCell;
            _isPlayerMoving = false;
            _pathService.ExtendPath(_targetCell);
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnMove -= HandleMoveInput;
            InputManager.Instance.OnConfirm -= HandleConfirmInput;
        }
    }
}