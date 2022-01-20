namespace BattleshipBasicTypes
{
    public struct BoardSquareState
    {
        public bool isEmpty { get; set; }
        public bool isBombed { get; set; }

        public BoardSquareState(bool empty, bool bombed)
        {
            isEmpty = empty;
            isBombed = bombed;
        }

        public override string ToString()
        {
            switch (isEmpty, isBombed)
            {
                case (false, false):
                    return "O";
                case (false, true):
                    return "X";
                case (true, false):
                    return "_";
                case (true, true):
                    return "x";
            }
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().IsInstanceOfType(obj))
            {
                return false;
            }

            var other = (BoardSquareState) obj;

            return isEmpty == other.isEmpty && isBombed == other.isBombed;
        }
    }
}