using System.Collections.Generic;
using BattleshipBasicTypes;

namespace Domain
{
    public class PlayerDTO
    {
        /*
        public BoardSquareState[][] MyBoard { get; set; } = new BoardSquareState[1][];
        public BoardSquareState[][] EnemyBoard { get; set; } = new BoardSquareState[1][];
        public Ship[] Ships { get; set; } = new Ship[1];*/

        public PlayerDTO()
        {
            
        }

        public List<List<BoardSquareState>> MyBoard { get; set; } = new ();
        public List<List<BoardSquareState>> EnemyBoard { get; set; } = new();
        public List<Ship> Ships { get; set; } = new ();
        
        public PlayerDTO(BoardSquareState[,] mine, BoardSquareState[,] enemy, List<Ship> ships)
        {
            MyBoard = makeBoard(mine);
            EnemyBoard = makeBoard(enemy);
            Ships = ships;
        }

        public static List<List<BoardSquareState>> makeBoard(BoardSquareState[,] board)
        {
            List<List<BoardSquareState>> serializable = new ();

            for (int y = 0; y < board.GetLength(1); y++)
            {
                List<BoardSquareState> serializableRow = new ();
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    serializableRow.Add(board[x, y]);
                }
                serializable.Add(serializableRow);
            }
            
            return serializable;
        }

        public BoardSquareState[,] RegenBoard(List<List<BoardSquareState>> boardList)
        {
            BoardSquareState[,] board = new BoardSquareState[boardList[0].Count, boardList.Count];

            for (int y = 0; y < board.GetLength(1); y++)
            {
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    board[x, y] = boardList[y][x];
                }
            }
            
            return board;
        }
    }
}