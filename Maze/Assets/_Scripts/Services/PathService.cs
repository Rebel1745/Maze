using System.Collections.Generic;
using System;
using UnityEngine;
using MazeGame.Data;

namespace MazeGame.Services
{
    public class PathService : IPathService
    {
        private readonly List<Vector3> _path = new();
        public IReadOnlyList<Vector3> CurrentPath => _path.AsReadOnly();

        public event Action<List<Vector3>> OnPathChanged;

        public void StartPath(MazeCell startCell)
        {
            _path.Clear();
            _path.Add(startCell.Centre);
            NotifyChange();
        }

        public void ExtendPath(MazeCell nextCell)
        {
            //if (_path.Count > 0 && _path[^1] == nextCell.Centre) return; // Avoid duplicates

            if (_path.Count > 1) _path.RemoveAt(_path.Count - 1);

            _path.Add(nextCell.Centre);
            NotifyChange();
        }

        public void UpdatePath(Vector3 pos, bool createTemp)
        {
            if (createTemp) _path.Add(pos);
            else _path[_path.Count - 1] = pos;

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
            OnPathChanged?.Invoke(new List<Vector3>(_path));
        }
    }
}