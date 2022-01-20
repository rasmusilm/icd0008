namespace BattleshipBasicTypes
{
    public class Move
    {
        public int Player { get; set; } = 0;
        public string MoveType { get; set; } = "move";
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public Move()
        {
            
        }
        
        public Move(int player, string moveType, int x, int y)
        {
            Player = player;
            MoveType = moveType;
            X = x;
            Y = y;
        }
    }
    
}