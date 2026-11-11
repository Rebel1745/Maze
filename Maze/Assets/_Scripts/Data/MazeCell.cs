namespace MazeGame.Data
{
    /// <summary>
    /// Represents a single cell in the maze grid.
    /// Pure data structure - no Unity types.
    /// </summary>
    public struct MazeCell
    {
        public int X;
        public int Y;
        public byte Walls; // Bitmask of WallMask constants
        public bool Visited;

        public bool HasWall(byte direction) => (Walls & direction) != 0;

        public void RemoveWall(byte direction) => Walls &= (byte)~direction;

        public void AddWall(byte direction) => Walls |= direction;

        public void ToggleWall(byte direction) => Walls ^= direction;

        public void SetVisited(bool visited) => Visited = visited;
    }
}