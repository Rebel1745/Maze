namespace MazeGame.Data
{
    /// <summary>
    /// Bitmask constants for wall presence in a MazeCell.
    /// </summary>
    public static class WallMask
    {
        public const byte None  = 0b0000;
        public const byte North = 0b0001;
        public const byte South = 0b0010;
        public const byte West  = 0b0100;
        public const byte East  = 0b1000;
        
        // Helper to get opposite direction for carving passages
        public static byte GetOpposite(byte direction)
        {
            return direction switch
            {
                North => South,
                South => North,
                West  => East,
                East  => West,
                _     => None
            };
        }
    }
}