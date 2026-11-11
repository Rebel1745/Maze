// Assets/Scripts/Presentation/MazeRenderer.cs
using UnityEngine;
using MazeGame.Data;
using MazeGame.Services;
using System.Collections.Generic;

namespace MazeGame.Presentation
{
    public class MazeRenderer : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private Sprite[] _wallSprites; // Array indexed by bitmask value (0-15)
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private Vector3 _originOffset = new Vector3(0, 0, 0); // To center the maze
        [SerializeField] private Material _sharedMaterial; // For batching

        [Header("References")]
        [SerializeField] private IMazeService _mazeService;

        private List<SpriteRenderer> _activeSprites = new();
        private MazeData _maze;

        /// <summary>
        /// Called by GameController to inject dependencies.
        /// </summary>
        public void Setup(IMazeService mazeService)
        {
            _mazeService = mazeService;
        }

        /// <summary>
        /// Renders the current maze data to the scene.
        /// </summary>
        public void RenderMaze()
        {
            if (_mazeService == null || _mazeService.CurrentMaze == null) return;

            ClearExisting();

            _maze = _mazeService.CurrentMaze;

            for (int x = 0; x < _maze.Width; x++)
            {
                for (int y = 0; y < _maze.Height; y++)
                {
                    SpawnWallSprite(x, y);
                }
            }
        }

        private void SpawnWallSprite(int x, int y)
        {
            MazeCell cell = _maze.GetCell(x, y);

            if (cell.Walls < 0 || cell.Walls >= _wallSprites.Length)
            {
                Debug.LogError($"Invalid wall mask {cell.Walls} for cell ({x},{y}). Ensure sprite array is sized correctly.");
                return;
            }

            GameObject go = new GameObject($"Wall_{x}_{y}_{cell.Walls}_{cell.Visited}");
            go.transform.SetParent(transform);

            // Calculate world position
            Vector3 worldPos = Utils.GridUtils.CellToWorld(new Vector3Int(x, y, 0), _cellSize, _originOffset);
            go.transform.position = worldPos;

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = _wallSprites[cell.Walls];
            sr.sharedMaterial = _sharedMaterial; // Critical for batching
            sr.sortingLayerName = "Walls";
            sr.sortingOrder = 0;

            _activeSprites.Add(sr);
        }

        private void ClearExisting()
        {
            foreach (var sr in _activeSprites)
            {
                if (sr != null) Destroy(sr.gameObject);
            }
            _activeSprites.Clear();
        }

        private void OnDestroy()
        {
            ClearExisting();
        }
    }
}