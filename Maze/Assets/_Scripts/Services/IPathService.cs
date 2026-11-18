using System.Collections.Generic;
using System;
using UnityEngine;
using MazeGame.Data;

namespace MazeGame.Services
{
    public interface IPathService
    {
        public event Action<List<Vector3>, List<Vector3>> OnPathChanged; // New Event

        public void StartPath(MazeCell startCell);
        public void ExtendPath(MazeCell nextCell);
        public void UpdatePath(Vector3 pos, MazeCell finalTarget, bool createTemp);
        public void ClearPaths();
    }
}