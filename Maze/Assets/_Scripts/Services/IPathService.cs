using System.Collections.Generic;
using System;
using UnityEngine;

namespace MazeGame.Services
{
    public interface IPathService
    {
        public event Action<List<Vector3Int>> OnPathChanged; // New Event

        public void StartPath(Vector3Int startCell);
        public void ExtendPath(Vector3Int nextCell);
        public void Backtrack();
        public void ClearPath();
        public IReadOnlyList<Vector3Int> CurrentPath { get; }
    }
}