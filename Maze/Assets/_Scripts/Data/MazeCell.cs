using UnityEngine;

namespace MazeGame.Data
{
    public class MazeCell
    {
        public int X;
        public int Y;
        public Vector3 Centre;
        public byte Walls; // Bitmask of WallMask constants
        public bool Visited;

        public bool HasWall(byte direction) => (Walls & direction) != 0;

        public void RemoveWall(byte direction) => Walls &= (byte)~direction;

        public void AddWall(byte direction) => Walls |= direction;

        public void ToggleWall(byte direction) => Walls ^= direction;

        public void SetVisited(bool visited) => Visited = visited;
    }
}