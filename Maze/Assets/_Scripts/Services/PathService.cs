using System.Collections.Generic;
using System;
using UnityEngine;

namespace MazeGame.Services
{
    public class PathService : IPathService
    {
        private readonly List<Vector3Int> _path = new();
        public IReadOnlyList<Vector3Int> CurrentPath => _path.AsReadOnly();

        public event Action<List<Vector3Int>> OnPathChanged;

        public void StartPath(Vector3Int startCell)
        {
            _path.Clear();
            _path.Add(startCell);
            NotifyChange();
        }

        public void ExtendPath(Vector3Int nextCell)
        {
            if (_path.Count > 0 && _path[^1] == nextCell) return; // Avoid duplicates
            
            _path.Add(nextCell);
            NotifyChange();
        }

        public void Backtrack()
        {
            if (_path.Count > 1)
            {
                _path.RemoveAt(_path.Count - 1);
                NotifyChange();
            }
        }

        public void ClearPath()
        {
            _path.Clear();
            NotifyChange();
        }

        private void NotifyChange()
        {
            OnPathChanged?.Invoke(new List<Vector3Int>(_path));
        }
    }
}