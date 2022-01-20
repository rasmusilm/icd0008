using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BattleshipBasicTypes;
using Domain;

namespace BattleshipBrain
{
    public class Player
    {
        
        private readonly BoardSquareState[,] _myBoard;
        private readonly BoardSquareState[,] _enemyBoard;
        private List<Ship> ships = new ();
        
        public Player(BoardSquareState[,] myBoard, BoardSquareState[,] enemyBoard)
        {
            _myBoard = myBoard;
            _enemyBoard = enemyBoard;
        }

        public BoardSquareState[,] getBoard(int designator)
        {
            switch (designator)
            {
                case 0:
                    return _myBoard;
                default:
                    return _enemyBoard;
            }
        }

        public void AddShip(Ship ship)
        {
            ships.Add(ship);
            foreach (var coordinate in ship.GetCoordinates())
            {
                _myBoard[coordinate.x, coordinate.y].isEmpty = false;
            }
        }

        public void RemoveShipAt(Coordinate coordinate)
        {
            Ship toRemove = null!;
            bool found = false;
            for (int i = 0; i < ships.Count; i++)
            {
                var ship = ships[i];
                Console.WriteLine(ship.GetShipSize());
                if (ship.GetCoordinates().Contains(coordinate))
                {
                    Console.WriteLine("removing");
                    toRemove = ship;
                    found = true;
                    foreach (var coord in ship.GetCoordinates())
                    {
                        _myBoard[coord.x, coord.y].isEmpty = true;
                    }
                    break;
                }
            }

            if (found) ships.Remove(toRemove);
        }

        public int[] BoardSize()
        {
            return new[] {_myBoard.GetLength(0), _myBoard.GetLength(1)};
        }

        public int getShipAmount()
        {
            return ships.Count;
        }

        public List<Ship> GetShips()
        {
            var shipList = new List<Ship>();
            foreach (var ship in ships)
            {
                shipList.Add(ship);
            }
            return shipList;
        }

        public bool CalculateLoss()
        {
            if (ships.Count == 0)
            {
                return false;
            }
            foreach (var ship in ships)
            {
                if (!ship.IsShipSunk(_myBoard))
                {
                    return false;
                }
            }
            return true;
        }

        public PlayerDTO serialize()
        {
            return new PlayerDTO(_myBoard, _enemyBoard, ships);
        }

        public static Player deserialize(PlayerDTO playerdata)
        {
            Player player =  new Player(playerdata.RegenBoard(playerdata.MyBoard), playerdata.RegenBoard(playerdata.EnemyBoard));
            player.ships = playerdata.Ships;
            return player;
        }

        public PlayerDbDTO transfer()
        {
            var dto = new PlayerDbDTO();
            dto.Ships = JsonSerializer.Serialize(ships, new JsonSerializerOptions());
            var regularDTO = serialize();
            dto.EnemyBoard = JsonSerializer.Serialize(regularDTO.EnemyBoard, new JsonSerializerOptions());
            dto.MyBoard = JsonSerializer.Serialize(regularDTO.MyBoard, new JsonSerializerOptions());

            return dto;
        }

        public static Player regenerate(PlayerDbDTO data)
        {
            var playerDto = new PlayerDTO();
            var shipText = data.Ships != null ? data.Ships! : "";
            var ships = JsonSerializer.Deserialize(shipText, typeof(List<Ship>), new JsonSerializerOptions());
            if (ships != null)
            {
                playerDto.Ships = (List<Ship>) ships;
            }
            var enemyText = data.EnemyBoard != null ? data.EnemyBoard! : "";
            var enemy = JsonSerializer.Deserialize(enemyText, typeof(List<List<BoardSquareState>>), new JsonSerializerOptions());
            if (enemy != null)
            {
                playerDto.EnemyBoard = (List<List<BoardSquareState>>) enemy;
            }
            var myText = data.MyBoard != null ? data.MyBoard! : "";
            var me = JsonSerializer.Deserialize(myText, typeof(List<List<BoardSquareState>>), new JsonSerializerOptions());
            if (me != null)
            {
                playerDto.MyBoard = (List<List<BoardSquareState>>) me;
            }

            return deserialize(playerDto);
        }
    }
}