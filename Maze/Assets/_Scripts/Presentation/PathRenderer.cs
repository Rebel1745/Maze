using UnityEngine;
using MazeGame.Services;
using System.Collections.Generic;

namespace MazeGame.Presentation
{
    public class PathRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LineRenderer _pathLineRenderer;
        private IPathService _pathService;

        public void Setup(IPathService pathService)
        {
            _pathService = pathService;

            // Subscribe to path changes
            _pathService.OnPathChanged += OnPathChanged;
        }

        private void OnPathChanged(List<Vector3> path)
        {
            RenderPath(path);
        }

        private void RenderPath(List<Vector3> path)
        {
            if (_pathLineRenderer == null) return;

            // 1. Clear previous path visuals
            ClearPreviousPath();

            if (path.Count == 0) return;

            _pathLineRenderer.positionCount = path.Count;

            // 2. Render new path
            for (int i = 0; i < path.Count; i++)
            {
                Vector3 pos = path[i];
                _pathLineRenderer.SetPosition(i, pos);
            }
        }

        private void ClearPreviousPath()
        {
            _pathLineRenderer.positionCount = 0;
            // if (_lastRenderedPath.Count > 0)
            // {
            //     foreach (Vector3Int pos in _lastRenderedPath)
            //     {
            //         _pathLineRenderer.SetTile(pos, null);
            //     }
            //     _lastRenderedPath.Clear();
            // }
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