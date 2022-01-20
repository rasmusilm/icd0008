namespace BattleshipBasicTypes
{
    public struct Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
        
        public Coordinate(int xCoord, int yCoord)
        {
            this.x = xCoord;
            this.y = yCoord;
        }

        public override string ToString()
        {
            return $"X: {x}; Y: {y}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Coordinate))
            {
                return false;
            }
            return (x == ((Coordinate)obj).x)
                   && (y == ((Coordinate)obj).y);
        }
    }
}