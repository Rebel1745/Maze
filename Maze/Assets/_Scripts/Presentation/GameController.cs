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

        private IMazeService _mazeService;
        private IPathService _pathService;

        private void Start()
        {
            // 1. Create Services
            _mazeService = new MazeService();
            _pathService = new PathService();

            // 2. Inject Services into Views
            if (_playerController != null)
            {
                _playerController.Setup(_mazeService, _pathService);
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
            GenerateNewMaze(_mazeWidth, _mazeHeight);
        }

        public void GenerateNewMaze(int width, int height)
        {
            _mazeService.GenerateNewMaze(width, height);
            _mazeRenderer.RenderMaze();
            _pathService.ClearPath();
            _pathRenderer.Clear();
            _playerController.SetupPlayer();
        }
    }
}