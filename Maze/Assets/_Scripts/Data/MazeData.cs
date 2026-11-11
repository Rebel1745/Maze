using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace MazeGame.Data
{
    /// <summary>
    /// The logical representation of the entire maze.
    /// Independent of rendering or physics.
    /// </summary>
    public class MazeData
    {
        private readonly MazeCell[,] _cells;

        public int Width { get; }
        public int Height { get; }

        public MazeData(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new MazeCell[width, height];

            // Initialize all cells with full walls
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _cells[x, y] = new MazeCell
                    {
                        X = x,
                        Y = y,
                        Walls = 0b1111,
                        Visited = false
                    };
                }
            }
        }

        public MazeCell GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new System.IndexOutOfRangeException($"Cell ({x},{y}) out of bounds");

            return _cells[x, y];
        }

        public void SetCell(int x, int y, MazeCell cell)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new System.IndexOutOfRangeException($"Cell ({x},{y}) out of bounds");

            _cells[x, y] = cell;
        }

        // Returns valid neighbors based on wall bitmask
        public List<MazeCell> GetOpenNeighbors(MazeCell cell)
        {
            var neighbors = new List<MazeCell>();

            if (!cell.HasWall(WallMask.North)) neighbors.Add(GetCell(cell.X, cell.Y + 1));
            if (!cell.HasWall(WallMask.South)) neighbors.Add(GetCell(cell.X, cell.Y - 1));
            if (!cell.HasWall(WallMask.East)) neighbors.Add(GetCell(cell.X + 1, cell.Y));
            if (!cell.HasWall(WallMask.West)) neighbors.Add(GetCell(cell.X - 1, cell.Y));

            return neighbors;
        }
    }
}