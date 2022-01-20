using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using BattleshipBasicTypes;
using Domain;
using Microsoft.CSharp.RuntimeBinder;

namespace BattleshipBrain
{
    public class BSBrain
    {

        private Player _player0;
        private Player _player1;
        private string _name = "";
        private GameConfig _config = new GameConfig();
        private int _turn = 0;
        private MoveHistory _history = new MoveHistory();
        
        public BSBrain(int xSize, int ySize)
        {
            _player0 = new Player(new BoardSquareState[xSize, ySize], new BoardSquareState[xSize, ySize]);
            _player1 = new Player(new BoardSquareState[xSize, ySize], new BoardSquareState[xSize, ySize]);

            MarkEmptyBoard(_player1.getBoard(0));
            MarkEmptyBoard(_player1.getBoard(1));
            MarkEmptyBoard(_player0.getBoard(0));
            MarkEmptyBoard(_player0.getBoard(1));
            
        }

        public BSBrain(GameConfig config)
        {
            _config = config;
            _player0 = new Player(new BoardSquareState[config.BoardHeight, config.BoardWidth],
                new BoardSquareState[config.BoardHeight, config.BoardWidth]);
            _player1 = new Player(new BoardSquareState[config.BoardHeight, config.BoardWidth],
                new BoardSquareState[config.BoardHeight, config.BoardWidth]);
            
            MarkEmptyBoard(_player1.getBoard(0));
            MarkEmptyBoard(_player1.getBoard(1));
            MarkEmptyBoard(_player0.getBoard(0));
            MarkEmptyBoard(_player0.getBoard(1));
        }

        public string getName()
        {
            Console.WriteLine(_name);
            return _name;
        }
        
        public void Rename(string name)
        {
            _name = name;
            Console.WriteLine(_name);
        }
        
        public void randomizeBoard(int playerId, int attempt = 0)
        {
            Random _rnd = new();
            var directions = new[] {"-x", "+x", "-y", "+y"};

            var count = 0;
            _config.ShipConfigs.Reverse();
            foreach (var shipConfig in _config.ShipConfigs)
            {
                count = 0;
                while (count < 100)
                {
                    if (shipConfig.Quantity - GetPlayersShips(playerId).Count(ship1 => ship1.Config.Name == shipConfig.Name) < 0)
                    {
                        break;
                    }
                    
                    try
                    {
                        var d = _rnd.Next(0, directions.Length+1);
                        PlaceShip(playerId, _rnd.Next(0, _config.BoardHeight), _rnd.Next(0, _config.BoardWidth), shipConfig,
                                                directions[d]);
                    }
                    catch (Exception e)
                    {
                        
                    }

                    count++;
                }
            }

            
            _config.ShipConfigs.Reverse();

            if (attempt < 100)
            {
                var done = true;
             
                foreach (var shipConfig in _config.ShipConfigs)
                {
                    if ((shipConfig.Quantity - GetPlayersShips(playerId).Count(ship1 => ship1.Config.Name == shipConfig.Name) != 0))
                    {
                        done = false;
                    }
                }
                if (!done)
                {
                    RemoveAllShips(playerId);
                    randomizeBoard(playerId, attempt + 1);
                }
            }

            
        }
        
        public void MarkEmptyBoard(BoardSquareState[,] board)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    BoardSquareState squareState = new BoardSquareState()
                    {
                        isBombed = false,
                        isEmpty = true
                    };
                    board[x, y] = squareState;
                }
            }
        }

        public void RemoveAllShips(int playerDesignator)
        {
            var player = playerDesignator % 2 == 0 ? _player0 : _player1;
            var toRemove = player.GetShips();
            foreach (var ship in toRemove)
            {
                player.RemoveShipAt(ship._coordinates[0]);
            }
        }

        public BoardSquareState[,] getBoard(int boardDesignator, int playerDesignator)
        {
            switch (playerDesignator)
            {
                case 0:
                    return copyOfBoard(_player0.getBoard(boardDesignator));
                default:
                    return copyOfBoard(_player1.getBoard(boardDesignator));
            }
        }


        private BoardSquareState[,] getTrueBoard(int boardDesignator, int playerDesignator)
        {
            switch (playerDesignator)
            {
                case 0:
                    return _player0.getBoard(boardDesignator);
                default:
                    return _player1.getBoard(boardDesignator);
            }
        }


        public BoardSquareState[,] bombBoardAt(int playerDesignator, int x, int y)
        {
            BoardSquareState[,] board = getTrueBoard(0, playerDesignator);

            if (x < board.GetLength(0) && y < board.GetLength(1))
            {
                if (!board[x, y].isBombed)
                {
                    board[x, y].isBombed = true;
                }
                else
                {
                    throw new RuntimeBinderException("Can't bomb the same field twice");
                }
            }
            else
            {
                throw new IndexOutOfRangeException("One or more of the coordinates are outside the board");
            }
            
            _history.Do("bomb", playerDesignator, x, y);

            BoardSquareState[,] boardToShow = syncBoard(playerDesignator, x, y, board[x, y]);

            return copyOfBoard(boardToShow);
        }

        private BoardSquareState[,] syncBoard(int playerDesignator, int x, int y, BoardSquareState absolute)
        {
            BoardSquareState[,] board = getTrueBoard(1, playerDesignator - 1);
            board[x, y] = absolute;

            return board;
        }

        private BoardSquareState[,] copyOfBoard(BoardSquareState[,] bord)
        {
            var newBoard = new BoardSquareState[bord.GetLength(0), bord.GetLength(1)];
            
            for (int x = 0; x < bord.GetLength(0); x++)
            {
                for (int y = 0; y < bord.GetLength(1); y++)
                {
                    newBoard[x, y] = bord[x, y];
                }
            }

            return newBoard;
        }

        public BoardSquareState[,] PlaceShip(int playerDesignator, int x, int y, ShipConfig type, string direction)
        {
            var player = playerDesignator % 2 == 0 ? _player0 : _player1;

            var boardSize = player.BoardSize();
            var exception = new ArgumentException("Ship won't fit onto the board");

            switch (direction)
            {
                case "-x":
                    if (x - type.SizeY < -1 | y + type.SizeX > boardSize[1])
                    {
                        throw exception;
                    }
                    break;
                case "+y":
                    if (x + type.SizeX > boardSize[0] | y + type.SizeY > boardSize[1])
                    {
                        throw exception;
                    }
                    break;
                case "-y":
                    if (x + type.SizeX > boardSize[0] | y - type.SizeY < -1)
                    {
                        throw exception;
                    }
                    break;
                default:
                    if (x + type.SizeY > boardSize[0] | y + type.SizeX > boardSize[1])
                    {
                        throw exception;
                    }
                    break;
            }

            var toCreate = new Ship(player.getShipAmount().ToString(), new Coordinate(x, y), type.SizeX, type.SizeY,
                direction);

            toCreate.Config = type;
            
            if (CheckOverLap(toCreate, player.getBoard(0)))
            {
                if (CheckPlacementRules(toCreate, player.getBoard(0)))
                {
                    if (type.Quantity - player.GetShips().Count(ship => ship.Config == type) > 0)
                    {
                        player.AddShip(toCreate);
                    }
                    
                }
            }
            

            return copyOfBoard(player.getBoard(0));
        }

        private bool CheckOverLap(Ship ship, BoardSquareState[,] board)
        {
            foreach (var coordinate in ship.GetCoordinates())
            {
                if (!board[coordinate.x, coordinate.y].isEmpty)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckPlacementRules(Ship ship, BoardSquareState[,] board)
        {
            if (_config.ShipPlacement != EShipPlacement.SideTouch)
            {
                var coords = ship.GetCoordinates();
                
                foreach (var coordinate in coords)
                {
                    var sideSouth = false;
                    var sideNorth = false;
                    var sideWest = false;
                    var sideEast = false;
                    
                    if (coordinate.x - 1 >= 0)
                    {
                        sideNorth = !board[coordinate.x - 1, coordinate.y].isEmpty;
                    }

                    if (coordinate.x + 1 < board.GetLength(0))
                    {
                        sideSouth = !board[coordinate.x + 1, coordinate.y].isEmpty;
                    }

                    if (coordinate.y + 1 < board.GetLength(1))
                    {
                        sideEast = !board[coordinate.x, coordinate.y + 1].isEmpty;
                    }

                    if (coordinate.y - 1 >= 0)
                    {
                        sideWest = !board[coordinate.x, coordinate.y - 1].isEmpty;
                    }
                    
                    if (sideEast | sideNorth | sideSouth | sideWest)
                    {
                        return false;
                    }
                    
                    if (_config.ShipPlacement == EShipPlacement.NoTouch)
                    {
                        var corner1 = false;
                        var corner2 = false;
                        var corner3 = false;
                        var corner4 = false;
                                        
                        if (coordinate.x - 1 >= 0)
                        {
                            if (coordinate.y + 1 < board.GetLength(1))
                            {
                                corner1 = !board[coordinate.x - 1, coordinate.y + 1].isEmpty;
                            }

                            if (coordinate.y - 1 >= 0)
                            {
                                corner2 = !board[coordinate.x - 1, coordinate.y - 1].isEmpty;
                            }
                        }
                    
                        if (coordinate.x + 1 < board.GetLength(0))
                        {
                            if (coordinate.y + 1 < board.GetLength(1))
                            {
                                corner3 = !board[coordinate.x + 1, coordinate.y + 1].isEmpty;
                            }

                            if (coordinate.y - 1 >= 0)
                            {
                                corner4 = !board[coordinate.x + 1, coordinate.y - 1].isEmpty;
                            }
                        }

                        if (corner1 | corner2 | corner3 | corner4)
                        {
                            return false;
                        }
                    }
                }

                
            }
            return true;
        }

        public List<Ship> GetPlayersShips(int playerDesignator)
        {
            var player = playerDesignator % 2 == 0 ? _player0 : _player1;
            return player.GetShips();
        }

        public void RemoveShip(int x, int y, int playerDesignator)
        {
            var player = playerDesignator % 2 == 0 ? _player0 : _player1;
            player.RemoveShipAt(new Coordinate(x, y));
        }

        public int FindWinner()
        {
            if (_player0.CalculateLoss())
            {
                return 1;
            }

            if (_player1.CalculateLoss())
            {
                return 0;
            }

            return -1;
        }

        public void Undo()
        {
            if (_history.GetUndoStackSize() == 0)
            {
                return;
            }
            var move = _history.Undo();
            switch (move.MoveType)
            {
                case "bomb":
                    var board = getTrueBoard(0, move.Player);
                    board[move.X, move.Y].isBombed = false;
                    var unsync = getTrueBoard(1, move.Player - 1);
                    unsync[move.X, move.Y] = new BoardSquareState(true, false);
                    break;
            }
        }

        public void Redo()
        {
            if (_history.GetRedoStackSize() == 0)
            {
                return;
            }
            var move = _history.Redo();
            switch (move.MoveType)
            {
                case "bomb":
                    bombBoardAt(move.Player, move.X, move.Y);
                    break;
            }
        }

        public MoveHistory GetHistory()
        {
            return _history;
        }

        public void SepHistory(MoveHistory newHistory)
        {
            _history = newHistory;
        }

        public void Continue()
        {
            while (_history.GetRedoStackSize() > 0)
            {
                Redo();
            }
        }

        public void ToBeginning()
        {
            while (_history.GetUndoStackSize() > 0)
            {
                Undo();
            }
        }

        public string GetBrainJSON()
        {
            BrainDTO DTOBrain = new BrainDTO(_player0.serialize(), _player1.serialize(), JsonSerializer.Serialize(_config));
            JsonSerializerOptions opt = new JsonSerializerOptions()
            {
                WriteIndented = true,
                AllowTrailingCommas = true,
            };
            string res = JsonSerializer.Serialize(DTOBrain.player1, opt);
            res += "\nB: " + JsonSerializer.Serialize(DTOBrain.player2, opt);

            return res;
        }

        public BrainDTO GetBrainDto()
        {
            var brainDto = new BrainDTO(_player0.serialize(), _player1.serialize(), JsonSerializer.Serialize(_config));
            brainDto.Name = _name;
            brainDto.History = JsonSerializer.Serialize(_history.GetDto());
            return brainDto;
        }

        public BrainDbDTO GetBrainDbDto()
        {
            var dto = new BrainDbDTO();
            dto.GameName = _name;
            dto.GameConfigDbDtoId = _config.Id;
            dto.Turn = _turn;

            return dto;
        }

        public PlayerDbDTO[] GetPlayerDbDtos()
        {
            var players = new PlayerDbDTO[2];
            players[0] = _player0.transfer();
            players[1] = _player1.transfer();

            return players;
        }

        public static BSBrain RegnenerateBrain(Player player1, Player player2)
        {
            BSBrain brain = new BSBrain(10, 10);
            brain._player0 = player1;
            brain._player1 = player2;
            return brain;
        }
        
        public static BSBrain RegnenerateBrain(BrainDTO info)
        {
            BSBrain brain = new BSBrain(10, 10);
            brain._player0 = Player.deserialize(info.player1);
            brain._player1 = Player.deserialize(info.player2);
            brain._config = JsonSerializer.Deserialize<GameConfig>(info.Config)!;
            brain._name = info.Name;
            brain._history = MoveHistory.getFromDto(JsonSerializer.Deserialize<MoveHistoryDTO>(info.History)!);
            return brain;
        }

        public static BSBrain RegnenerateBrain(BrainDbDTO data, Player p1, Player p2)
        {
            BSBrain brain = new BSBrain(10, 10);
            brain._player0 = p1;
            brain._player1 = p2;
            brain._turn = data.Turn;
            brain._name = data.GameName!;
            return brain;
        }
        
        public static BSBrain RegnenerateBrain(GameConfig gameConfig, Player p1, Player p2)
        {
            BSBrain brain = new BSBrain(gameConfig);
            brain._player0 = p1;
            brain._player1 = p2;
            return brain;
        }

        public int nextTurn()
        {
            _turn = (_turn + 1) % 2;
            return _turn + 0;
        }

        public int currentTurn()
        {
            return _turn + 0;
        }

        public GameConfig getConfig()
        {
            return _config;
        }
    }
}