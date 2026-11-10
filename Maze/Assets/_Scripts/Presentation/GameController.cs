// Assets/Scripts/Presentation/GameController.cs
using UnityEngine;
using MazeGame.Services;

namespace MazeGame.Presentation
{
    public class GameController : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private MazeRenderer _mazeRenderer;
        [SerializeField] private PathRenderer _pathRenderer;

        [Header("Setup")]
        [SerializeField] private int _mazeWidth = 10;
        [SerializeField] private int _mazeHeight = 10;


        private IInputService _inputService;
        private IMazeService _mazeService;
        private IPathService _pathService;

        private void Awake()
        {
            // 1. Create Services
            _inputService = new InputService();
            _mazeService = new MazeService();
            _pathService = new PathService();

            // 2. Inject Services into Views
            if (_playerController != null)
            {
                _playerController.Setup(_inputService, _mazeService, _pathService);
            }

            if (_mazeRenderer != null)
            {
                _mazeRenderer.Setup(_mazeService);
            }

            if (_pathRenderer != null)
            {
                _pathRenderer.Setup(_pathService);
            }

            // 3. Start Game
            _inputService.Enable();
            GenerateNewMaze(_mazeWidth, _mazeHeight);
        }

        private void OnDestroy()
        {
            _inputService.Dispose();
        }

        public void GenerateNewMaze(int width, int height)
        {
            _mazeService.GenerateNewMaze(width, height);
            _mazeRenderer.RenderMaze();
            _pathRenderer.Clear();
            //_pathService.ClearPath();
        }
    }
}