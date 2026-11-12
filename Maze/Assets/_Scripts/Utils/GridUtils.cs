// Assets/Scripts/Utils/GridUtils.cs
using UnityEngine;

namespace MazeGame.Utils
{
    public static class GridUtils
    {
        public static Vector3Int WorldToCell(Vector3 worldPos, float cellSize, Vector3 originOffset = default)
        {
            Vector3 relative = worldPos - originOffset;
            int x = Mathf.FloorToInt(relative.x / cellSize);
            int y = Mathf.FloorToInt(relative.y / cellSize);
            return new Vector3Int(x, y, 0);
        }

        public static Vector3 CellToWorld(Vector3Int cell, float cellSize, Vector3 originOffset = default)
        {
            return new Vector3(
                (cell.x + 0.5f) * cellSize + originOffset.x,
                (cell.y + 0.5f) * cellSize + originOffset.y,
                originOffset.z
            );
        }
    }
}