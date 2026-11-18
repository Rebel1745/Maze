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

        private const float UPDATE_THRESHOLD = 0.1f;

        public void StartPath(MazeCell startCell)
        {
            _path.Clear();
            _path.Add(startCell.Centre);
            NotifyChange();
        }

        public void ExtendPath(MazeCell nextCell)
        {
            // remove the temporary position and then add the new cell on
            if (_path.Count > 1) _path.RemoveAt(_path.Count - 1);

            // avoid dupes
            if (_path.Count > 1 && _path[_path.Count - 1] == nextCell.Centre) return;

            _path.Add(nextCell.Centre);
            NotifyChange();
        }

        // Creates or updates a temporary point so the line renderer follows the players position 
        // rather than only create a point in the line renderer when a new maze cell is reached
        public void UpdatePath(Vector3 pos, MazeCell finalTarget, bool createTemp)
        {
            // if we have backtracked, remove the previous line renderer position
            if (createTemp && _path.Count > 1 && _path[^2] == finalTarget.Centre) createTemp = false;

            // createTemp is true when a new movement has just started, it is false when the player is still moving towards the target
            if (createTemp) _path.Add(pos);
            else if (Vector3.Distance(_path[^1], pos) > UPDATE_THRESHOLD) _path[_path.Count - 1] = pos;

            NotifyChange();
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