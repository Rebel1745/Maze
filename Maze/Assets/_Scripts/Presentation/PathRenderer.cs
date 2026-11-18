using UnityEngine;
using MazeGame.Services;
using System.Collections.Generic;

namespace MazeGame.Presentation
{
    public class PathRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LineRenderer _currentPathLineRenderer;
        [SerializeField] private LineRenderer _totalPathLineRenderer;
        private IPathService _pathService;

        public void Setup(IPathService pathService)
        {
            _pathService = pathService;

            // Subscribe to path changes
            _pathService.OnPathChanged += OnPathChanged;
        }

        private void OnPathChanged(List<Vector3> currentPath, List<Vector3> totalPath)
        {
            RenderPath(currentPath, totalPath);
        }

        private void RenderPath(List<Vector3> currentPath, List<Vector3> totalPath)
        {
            if (_currentPathLineRenderer == null || _totalPathLineRenderer == null) return;

            // 1. Clear previous path visuals
            ClearPreviousPaths();

            if (currentPath.Count == 0 || totalPath.Count == 0) return;

            _currentPathLineRenderer.positionCount = currentPath.Count;
            _totalPathLineRenderer.positionCount = totalPath.Count;

            // Render new current path
            for (int i = 0; i < currentPath.Count; i++)
            {
                Vector3 pos = currentPath[i];
                _currentPathLineRenderer.SetPosition(i, pos);
            }

            // Render new total path
            for (int i = 0; i < totalPath.Count; i++)
            {
                Vector3 pos = totalPath[i];
                _totalPathLineRenderer.SetPosition(i, pos);
            }
        }

        private void ClearPreviousPaths()
        {
            _currentPathLineRenderer.positionCount = 0;
            _totalPathLineRenderer.positionCount = 0;
        }

        public void Clear()
        {
            ClearPreviousPaths();
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