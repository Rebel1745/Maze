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

        public void GenerateNewMaze(int width, int height)
        {
            CurrentMaze = new MazeData(width, height);
            // TODO: Call Generator strategy here
        }

        public MazeCell GetCellAt(int x, int y) => CurrentMaze.GetCell(x, y);
        
        public bool IsWalkable(int x, int y) => true; // Logic depends on rules
    }
}