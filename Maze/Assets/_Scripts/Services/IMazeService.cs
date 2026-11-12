using MazeGame.Data;
using Unity.Mathematics;

namespace MazeGame.Services
{
    public interface IMazeService
    {
        public MazeData CurrentMaze { get; }
        public void GenerateNewMaze(int width, int height);
        public MazeCell GetCellAt(int x, int y);
        public bool CellIsValid(int x, int y);
        public bool IsWalkable(int x, int y);
    }
}