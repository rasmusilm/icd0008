using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipBasicTypes
{
    public class Ship
    {
        public string _name { get; set; } = "";
        public List<Coordinate> _coordinates { get; set; } = new();

        public ShipConfig Config { get; set; } = new();

        public int GetShipSize() => _coordinates.Count;

        public Ship()
        {
            
        }

        public Ship(string name, Coordinate position, int lenght, int height, string direction)
        {
            Console.WriteLine(direction);
            _name = name;
            switch (direction)
            {
                case "-y":
                    for (int x = position.x; x < position.x + lenght; x++)
                    {
                        for (int y = position.y; y > position.y - height; y--)
                        {
                            Console.Write(x+":"+y);
                            _coordinates.Add(new Coordinate(x, y));
                        }
                    }
                    break;
                case "+x":
                    for (int y = position.y; y < position.y + lenght; y++)
                    {
                        for (int x = position.x; x < position.x + height; x++)
                        {
                            Console.Write(x+":"+y);
                            _coordinates.Add(new Coordinate(x, y));
                        }
                    }
                    break;
                case "-x":
                    for (int y = position.y; y < position.y + lenght; y++)
                    {
                        for (int x = position.x; x > position.x - height; x--)
                        {
                            Console.Write(x+":"+y);
                            _coordinates.Add(new Coordinate(x, y));
                        }
                    }
                    break;
                default:
                    for (int x = position.x; x < position.x + lenght; x++)
                    {
                        for (int y = position.y; y < position.y + height; y++)
                        {
                            Console.Write(x+":"+y);
                            _coordinates.Add(new Coordinate(x, y));
                        }
                    }
                    break;
            }
        }

        public int GetShipDamage(BoardSquareState[,] board)
        {
            var count = 0;
            foreach (var coordinate in _coordinates)
            {
                if (board[coordinate.x, coordinate.y].isBombed) count++;
            }

            return count;
        }

        public List<Coordinate> GetCoordinates()
        {
            var coords = new List<Coordinate>();
            foreach (var square in _coordinates)
            {
                
                coords.Add(new Coordinate(square.x, square.y));
            }

            return coords;
        }

        public string GetName() => _name;

        public bool IsShipSunk(BoardSquareState[,] board) =>
            _coordinates.All(coordinate => board[coordinate.x, coordinate.y].isBombed);
    }
}
        