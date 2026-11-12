using UnityEngine;
using UnityEngine.Tilemaps;
using MazeGame.Services;
using System.Collections.Generic;

namespace MazeGame.Presentation
{
    public class PathRenderer : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private Tile _straightTile;
        [SerializeField] private Tile _cornerTile;
        [SerializeField] private Tile _endTile;
        [SerializeField] private Tile _startTile;

        [Header("References")]
        [SerializeField] private Tilemap _pathTilemap;
        [SerializeField] private IPathService _pathService;

        private List<Vector3Int> _lastRenderedPath = new();

        public void Setup(IPathService pathService)
        {
            _pathService = pathService;

            // Subscribe to path changes
            _pathService.OnPathChanged += OnPathChanged;
        }

        private void OnPathChanged(List<Vector3Int> path)
        {
            RenderPath(path);
        }

        private void RenderPath(List<Vector3Int> path)
        {
            if (_pathTilemap == null) return;

            // 1. Clear previous path visuals
            ClearPreviousPath();

            if (path.Count == 0) return;

            // 2. Render new path
            for (int i = 0; i < path.Count; i++)
            {
                Vector3Int pos = path[i];
                Tile tileToPlace = _straightTile; // Default

                // Determine tile type based on index and neighbors
                if (i == 0)
                {
                    tileToPlace = _startTile;
                }
                else if (i == path.Count - 1)
                {
                    tileToPlace = _endTile;
                }
                else
                {
                    // Check if it's a corner or straight
                    Vector3Int prev = path[i - 1];
                    Vector3Int next = path[i + 1];

                    // If direction changes, it's a corner
                    if (prev.x != next.x && prev.y != next.y)
                    {
                        tileToPlace = _cornerTile;
                    }
                    else
                    {
                        tileToPlace = _straightTile;
                    }
                }

                _pathTilemap.SetTile(pos, tileToPlace);
            }

            _lastRenderedPath = new List<Vector3Int>(path);
        }

        private void ClearPreviousPath()
        {
            if (_lastRenderedPath.Count > 0)
            {
                foreach (Vector3Int pos in _lastRenderedPath)
                {
                    _pathTilemap.SetTile(pos, null);
                }
                _lastRenderedPath.Clear();
            }
        }

        public void Clear()
        {
            ClearPreviousPath();
        }

        private void OnDestroy()
        {
            if (_pathService != null)
            {
                _pathService.OnPathChanged -= OnPathChanged;
            }
        }
    }
}