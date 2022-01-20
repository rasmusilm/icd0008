using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BattleshipBasicTypes;

namespace BattleshipBrain
{
    public class tmp
    {
        public static BSBrain Game = new BSBrain(10, 10);

        public BoardSquareState[,] Own = Game.getBoard(0, 0);

        public BoardSquareState[,] Enemy = Game.getBoard(1, 0);

        public int Id = 0;

        public int Player = 0;

        public List<Ship> Ships = Game.GetPlayersShips(0);

        public bool loadFromDB = Functions.useDB();

        public string name = "";

        public string mode = "bomb";

        public GameConfig? Config;

        public string placeDirection = "r";

        public string shiptypeToPlace = "Destroyer";

        public bool delete = false;

        public bool GameOver = false;

        public int Winner = -1;

        public tmp(int x, int y, int s, string id, bool load, int player, string direct, string t,
            bool startNew, bool delete, string rewind)
        {
            if (loadFromDB)
            {
                if (startNew)
                {
                    Id = Functions.start(int.Parse(id));
                    Game = DataHandler.ReadSaveGameDB(Id);
                    Own = Game.getBoard(0, player);
                    Enemy = Game.getBoard(1, player);
                    id = Id.ToString();
                }

                var gameId = int.Parse(id, NumberStyles.AllowLeadingSign);
                if (gameId == -1)
                {
                    throw new IndexOutOfRangeException();
                    Game = new BSBrain(10, 10);
                }
                else if (load)
                {
                    Game = DataHandler.ReadSaveGameDB(gameId);
                    Own = Game.getBoard(0, player);
                    Enemy = Game.getBoard(1, player);
                    Id = gameId;
                }

                if (!string.IsNullOrEmpty(rewind))
                {
                    switch (rewind)
                    {
                        case "undo":
                            Game.Undo();
                            Own = Game.getBoard(0, player);
                            Enemy = Game.getBoard(1, player);
                            break;
                        case "redo":
                            Game.Redo();
                            Own = Game.getBoard(0, player);
                            Enemy = Game.getBoard(1, player);
                            break;
                        case "continue":
                            Game.Continue();
                            Own = Game.getBoard(0, player);
                            Enemy = Game.getBoard(1, player);
                            break;
                        case "begin":
                            Game.ToBeginning();
                            Own = Game.getBoard(0, player);
                            Enemy = Game.getBoard(1, player);
                            break;
                    }

                    DataHandler.UpdateSaveGameDB(Game, gameId);
                }

                string[] directions = new[] {"u", "d", "l", "r"};

                if (!string.IsNullOrEmpty(direct) && directions.Contains(direct))
                {
                    placeDirection = direct;
                }

                if (!string.IsNullOrEmpty(t))
                {
                    shiptypeToPlace = t;
                }

                if (delete)
                {
                    this.delete = delete;
                }

                if (s == 2)
                {
                    mode = "place";
                    Config = Game.getConfig();
                    Ships = Game.GetPlayersShips(player);
                }
                else if (s == 3)
                {
                    mode = "place";
                    Config = Game.getConfig();
                    if (delete)
                    {
                        Game.RemoveShip(x, y, player);
                        Own = Game.getBoard(0, player);
                    }
                    else
                    {
                        Ships = Game.GetPlayersShips(player);
                        foreach (var shipConfig in Config.ShipConfigs)
                        {
                            if (shipConfig.Name == shiptypeToPlace)
                            {
                                if (shipConfig.Quantity - Ships.Count(ship1 => ship1.Config.Name == shipConfig.Name) >
                                    0)
                                {
                                    Console.WriteLine("placing ship");
                                    var directionDesignators = new string[] {"-y", "+y", "-x", "+x"};
                                    Own = Game.PlaceShip(player, x, y,
                                        Config.ShipConfigs.Find(c => c.Name == shiptypeToPlace)!,
                                        directionDesignators[Array.IndexOf(directions, placeDirection)]);

                                }
                            }
                        }
                    }

                    Ships = Game.GetPlayersShips(player);
                    DataHandler.UpdateSaveGameDB(Game, gameId);
                }

                if (s == 0)
                {
                    Enemy = Game.getBoard(1, player);
                }
                else if (s == 1)
                {
                    try
                    {
                        Enemy = Game.bombBoardAt((player + 1) % 2, x, y);
                    }
                    catch (Exception e)
                    {
                        Enemy = Game.getBoard(1, player);
                    }

                    Own = Game.getBoard(0, player);
                    DataHandler.UpdateSaveGameDB(Game, gameId);
                }

                Player = player;
            }
        }

        public class Functions
        {
            public static int start(int i)
            {
                return 1;
            }

            public static bool useDB()
            {
                return true;
            }
        }
    }
}

    