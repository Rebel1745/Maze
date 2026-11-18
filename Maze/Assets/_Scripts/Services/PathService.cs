using System.Collections.Generic;
using System;
using UnityEngine;
using MazeGame.Data;

namespace MazeGame.Services
{
    public class PathService : IPathService
    {
        private readonly List<Vector3> _currentPath = new();
        private readonly List<Vector3> _totalPath = new();

        public event Action<List<Vector3>, List<Vector3>> OnPathChanged;

        private const float UPDATE_THRESHOLD = 0.15f;

        public void StartPath(MazeCell startCell)
        {
            _currentPath.Clear();
            _currentPath.Add(startCell.Centre);
            _totalPath.Clear();
            _totalPath.Add(startCell.Centre);
            NotifyChange();
        }

        public void ExtendPath(MazeCell nextCell)
        {
            // remove the temporary position and then add the new cell on
            if (_currentPath.Count > 1) _currentPath.RemoveAt(_currentPath.Count - 1);

            _totalPath.Add(nextCell.Centre);

            // avoid dupes
            if (_currentPath.Count > 1 && _currentPath[_currentPath.Count - 1] == nextCell.Centre) return;

            _currentPath.Add(nextCell.Centre);
            NotifyChange();
        }

        // Creates or updates a temporary point so the line renderer follows the players position 
        // rather than only create a point in the line renderer when a new maze cell is reached
        public void UpdatePath(Vector3 pos, MazeCell finalTarget, bool createTemp)
        {
            // if we have backtracked, don't create a new position on the line, just update the current one
            if (createTemp && _currentPath.Count > 1 && _currentPath[^2] == finalTarget.Centre) createTemp = false;

            // createTemp is true when a new movement has just started, it is false when the player is still moving towards the target
            if (createTemp) _currentPath.Add(pos);
            else if (Vector3.Distance(_currentPath[^1], pos) > UPDATE_THRESHOLD) _currentPath[_currentPath.Count - 1] = pos;

            NotifyChange();
        }

        public void ClearPaths()
        {
            _currentPath.Clear();
            _totalPath.Clear();
            NotifyChange();
        }

        private void NotifyChange()
        {
            OnPathChanged?.Invoke(new List<Vector3>(_currentPath), new List<Vector3>(_totalPath));
        }
    }
}