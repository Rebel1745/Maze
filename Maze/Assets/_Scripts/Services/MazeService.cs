using System;
using System.Collections.Generic;
using System.Diagnostics;
using MazeGame.Data;

namespace MazeGame.Services
{
    /// <summary>
    /// Handles maze generation and data access.
    /// Plain C# class - no MonoBehaviour needed unless it needs Update().
    /// </summary>
    public class MazeService : IMazeService
    {
        public MazeData CurrentMaze { get; private set; }

        private bool _courseComplete = false;
        private int _currentX;
        private int _currentY;
        private readonly List<byte> _neighbours = new(4);

        public void GenerateNewMaze(int width, int height)
        {
            CurrentMaze = new MazeData(width, height);
            _courseComplete = false;
            _currentX = 0;
            _currentY = 0;
            MazeCell start = GetCellAt(_currentX, _currentY);
            start.SetVisited(true);
            SetCellAt(_currentX, _currentY, start);

            int tries = 0;
            while (!_courseComplete && tries < 1000000)
            {
                Kill(); // Will run until it hits a dead end.
                Hunt(); // Finds the next unvisited cell with an adjacent visited cell. If it can't find any, it sets courseComplete to true.

                tries++;
                if (tries > 999999) UnityEngine.Debug.Log("GenerateNewMaze exceeded the tries limit");
            }
        }

        private void Kill()
        {
            int tries = 0;
            while (tries < 1000000)
            {
                _neighbours.Clear();

                if (CellIsNotVisited(_currentX, _currentY + 1)) _neighbours.Add(WallMask.North);
                if (CellIsNotVisited(_currentX, _currentY - 1)) _neighbours.Add(WallMask.South);
                if (CellIsNotVisited(_currentX + 1, _currentY)) _neighbours.Add(WallMask.East);
                if (CellIsNotVisited(_currentX - 1, _currentY)) _neighbours.Add(WallMask.West);

                if (_neighbours.Count == 0) return;

                CarveFromCurrent(_neighbours[UnityEngine.Random.Range(0, _neighbours.Count)]);

                tries++;
                if (tries > 999999) UnityEngine.Debug.Log("GenerateNewMaze exceeded the tries limit");
            }
        }

        private void CarveFromCurrent(byte dir)
        {
            MazeCell current = GetCellAt(_currentX, _currentY);
            int nx = _currentX, ny = _currentY;

            switch (dir)
            {
                case WallMask.North: ny++; break;
                case WallMask.South: ny--; break;
                case WallMask.East: nx++; break;
                case WallMask.West: nx--; break;
                default: return;
            }

            current.RemoveWall(dir);
            SetCellAt(current.X, current.Y, current);
            MazeCell next = GetCellAt(nx, ny);
            next.RemoveWall(WallMask.GetOpposite(dir));

            _currentX = nx;
            _currentY = ny;
            next.SetVisited(true); // Mark on ENTRY, not on exit.
            SetCellAt(next.X, next.Y, next);
        }

        private void Hunt()
        {
            _courseComplete = true; // Set it to this, and see if we can prove otherwise below!

            for (int y = 0; y < CurrentMaze.Height; y++)
            {
                for (int x = 0; x < CurrentMaze.Width; x++)
                {
                    if (!GetCellAt(x, y).Visited && CellHasAnAdjacentVisitedCell(x, y))
                    {
                        _courseComplete = false; // Yep, we found something so definitely do another Kill cycle.
                        _currentX = x;
                        _currentY = y;
                        DestroyAdjacentWall(_currentX, _currentY);
                        MazeCell cell = GetCellAt(_currentX, _currentY);
                        cell.SetVisited(true);
                        SetCellAt(_currentX, _currentY, cell);
                        return; // Exit the function
                    }
                }
            }
        }

        private bool CellHasAnAdjacentVisitedCell(int x, int y)
        {
            int visitedCells = 0;

            // Look 1 row up (north)
            if (CellIsVisited(x, y + 1)) visitedCells++;

            // Look one row down (south)
            if (CellIsVisited(x, y - 1)) visitedCells++;

            // Look one row left (west)
            if (CellIsVisited(x - 1, y)) visitedCells++;

            // Look one row right (east)
            if (CellIsVisited(x + 1, y)) visitedCells++;

            // return true if there are any adjacent visited cells to this one
            return visitedCells > 0;
        }

        private void DestroyAdjacentWall(int x, int y)
        {
            _neighbours.Clear();

            if (CellIsVisited(x, y + 1)) _neighbours.Add(WallMask.North);
            if (CellIsVisited(x, y - 1)) _neighbours.Add(WallMask.South);
            if (CellIsVisited(x + 1, y)) _neighbours.Add(WallMask.East);
            if (CellIsVisited(x - 1, y)) _neighbours.Add(WallMask.West);

            if (_neighbours.Count == 0)
            {
                UnityEngine.Debug.LogWarning($"DestroyAdjacentWall: no visited neighbour at ({x},{y})");
                return;
            }

            byte dir = _neighbours[UnityEngine.Random.Range(0, _neighbours.Count)];

            int nx = x, ny = y;
            switch (dir)
            {
                case WallMask.North: ny++; break;
                case WallMask.South: ny--; break;
                case WallMask.East: nx++; break;
                case WallMask.West: nx--; break;
            }

            MazeCell current = GetCellAt(x, y);
            MazeCell next = GetCellAt(nx, ny);

            current.RemoveWall(dir);
            SetCellAt(current.X, current.Y, current);
            next.RemoveWall(WallMask.GetOpposite(dir));
            SetCellAt(next.X, next.Y, next);
        }

        private bool CellIsNotVisited(int x, int y) => CellIsValid(x, y) && !GetCellAt(x, y).Visited;
        private bool CellIsVisited(int x, int y) => CellIsValid(x, y) && GetCellAt(x, y).Visited;

        private bool CellIsValid(int x, int y) => x >= 0 && x < CurrentMaze.Width && y >= 0 && y < CurrentMaze.Height;

        public MazeCell GetCellAt(int x, int y) => CurrentMaze.GetCell(x, y);
        private void SetCellAt(int x, int y, MazeCell cell) => CurrentMaze.SetCell(x, y, cell);

        public bool IsWalkable(int x, int y) => true; // Logic depends on rules
    }
}