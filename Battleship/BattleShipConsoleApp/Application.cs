using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using BattleshipBasicTypes;
using BattleshipBrain;
using BattleshipConsoleUI;
using DAL;
using MenuLibrary;
using Microsoft.CSharp.RuntimeBinder;

namespace BattleShipConsoleApp
{
    public class Application
    {

        private BSBrain _brain;
        private GameConfig _config;
        private bool _gameActive;
        private bool[] _shipsPlaced = {false, false};
        private List<GameConfig> _configs = new();
        public string ConfigPath = "";
        public string SaveGamePath = "";
        private Menu _mainmenu = new Menu(EMenuLevel.root, "");
        private bool _saveLocaly = true;
        private string _id = "";
        public Application(GameConfig defaultConf)
        {
            _config = defaultConf;
            _brain = new (defaultConf.BoardWidth, defaultConf.BoardHeight);
            _configs.Add(defaultConf);
            Console.WriteLine(_configs.Count);
        }

        private readonly List<string> _menuSpecialShortcuts = new()
        {
            Menu.menuItemExit.Shortcut, 
            Menu.menuItemMain.Shortcut
        };

        private List<GameConfig> LoadExistingConfigs()
        {
            List<GameConfig> loaded = new();
            string[] files = Directory.GetFiles(ConfigPath);
            int counter = 1;
            foreach (var file in files)
            {
                var conf = DataHandler.ReadConfigFromJsonFile(file);
                loaded.Add(conf);
            }
            return loaded;
        }

        private Dictionary<string, string> GetConfigNameDictionary()
        {
            Dictionary<string, string> configs_by_name = new();
            if (Functions.useDB())
            {
                configs_by_name = DataHandler.ReadConfigNamesIdDB();
            }
            else
            {
                configs_by_name = DataHandler.LoadConfigsNamesFilenamesJson(Functions.GetConfigurationJsonPath());
            }

            return configs_by_name;
        }

        private Dictionary<string, BSBrain> getSavesDictionary()
        {
            Dictionary<string, BSBrain> saves = new();
            string[] files = Directory.GetFiles(SaveGamePath);
            int counter = 1;
            foreach (var file in files)
            {
                var conf = DataHandler.ReadSaveGame(file);
                var name = file.Split(Path.DirectorySeparatorChar);
                Console.WriteLine(name);
                saves.Add(name[name.Length-1], conf);
            }

            return saves;
        }

        public void Run()
        {
            _configs = LoadExistingConfigs();
            
            _mainmenu = new Menu(EMenuLevel.root, "Main menu");

            _mainmenu.Background = ConsoleColor.Blue;
            _mainmenu.TextColor = ConsoleColor.Gray;
            
            _mainmenu.AddMenuItems(new[]
            {
                new MenuItem("L", "Load game", LoadMenu),
                new MenuItem("N", "New game", NewGame),
                new MenuItem("A", "Configurations", ConfMenu),
                new MenuItem("O", "Options", SettingsMenu)
            });

            _mainmenu.Run();
        }

        public string CurrentGame()
        {
            
            var currentGameMenu = new Menu(EMenuLevel.special, "Battleship");
            currentGameMenu.AddMenuItems(new []
            {
                new MenuItem("A", "Player 1", Player1),
                new MenuItem("B", "Player 2", Player2)
            });

            var output = currentGameMenu.Run();
            
            return output;
        }

        private string Player1()
        {
            var p1Menu = new Menu(EMenuLevel.secondary, "Player 1 actions");
            p1Menu.AddMenuItems(new []
            {
                new MenuItem("V", "My board", BoardViewPlayer1),
                new MenuItem("B", "Bomb other player", BombingView1),
                new MenuItem("P", "Place ships", ShipPlacement1)
            });
            return p1Menu.Run();
        }
        
        
        private string Player2()
        {
            var p2Menu = new Menu(EMenuLevel.secondary, "Player 2 actions");
            p2Menu.AddMenuItems(new []
            {
                new MenuItem("V", "My board", BoardViewPlayer2),
                new MenuItem("B", "Bomb other player", BombingView2),
                new MenuItem("P", "Place ships", ShipPlacement2)
            });
            return p2Menu.Run();
        }

        private string ShipPlacement1()
        {
            return ShipPlacement(0);
        }
        
        private string ShipPlacement2()
        {
            return ShipPlacement(1);
        }
        
        private string ShipPlacement(int player)
        {
            int currentSelection = 0;
            ShipConfig ship = _brain.getConfig().ShipConfigs[0];
            string direction = "+x";

            var directionSigns = new string[]
            {
                "↑", "↓", "←", "→"
            };
            
            var directionDesignators = new string[] {"-y", "+y", "-x", "+x"};

            ConsoleKey key;
            
            do
            {
                var options = new string[]
                {
                    "Ship: " + ship.Name,
                    "Direction: " + directionSigns[Array.IndexOf(directionDesignators, direction)],
                    "Place",
                    "Randomize",
                    "Remove"
                };
                
                Console.Clear();
                var board = _brain.getBoard(0, player);
                BSConsoleUI.DrawBoard(board);
                
                for (int i = 0; i < options.Length; i++)
                {
                    var atm = Console.ForegroundColor;

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine(options[i]);

                    Console.ForegroundColor = atm;
                }
                
                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Escape:
                        if (Functions.useDB())
                        {
                            var id = 0;
                            if (int.TryParse(_id, out id))
                            {
                                DataHandler.UpdateSaveGameDB(_brain, id);
                            }
                            else
                            {
                                _id = DataHandler.SaveGameToDB(_brain).ToString();
                            }
                        }
                        else
                        {
                            var id = 0;
                            if (int.TryParse(_id, out id))
                            {
                                DataHandler.SaveGameToJson(_brain, Functions.GetGameJsonPath(), id+".json");
                                _id += ".json";
                            }
                            else
                            {
                                DataHandler.SaveGameToJsonOverwrite(_brain, Functions.GetGameJsonPath(), _id);
                            }
                        }
                        return "";
                    case ConsoleKey.UpArrow:
                        if (currentSelection >= 1)
                        {
                            currentSelection -= 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentSelection + 1 < options.Length)
                        {
                            currentSelection += 1;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (currentSelection)
                        {
                            case 2:
                                place(player, ship, direction);
                                break;
                            case 0:
                                ship = chooseShip(player);
                                break;
                            case 1:
                                direction = chooseDirection();
                                break;
                            case 3:
                                _brain.randomizeBoard(player);
                                break;
                            case 4:
                                Remove(player);
                                break;
                        }

                        break;
                }
                
                
            } while (true);
            

            return "";
        }

        private void place(int player, ShipConfig ship, string direction)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            
            Console.Write("Insert column: ");
            
            var m = Console.ReadLine()?.Trim();

            var x = alphabet.IndexOf(m!.ToLower(), StringComparison.Ordinal);
            
            Console.Write("Insert row: ");
            
            var n = Console.ReadLine()?.Trim();
            
            int.TryParse(n, out var y);
            try
            {
                _brain.PlaceShip(player, x, y-1, ship, direction);
            }
            catch (Exception e)
            {
                Console.WriteLine("invalid coordinates: " + m + "" + y);
            }
        }
        
        private void Remove(int player)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            
            Console.Write("Insert column: ");
            
            var m = Console.ReadLine()?.Trim();

            var x = alphabet.IndexOf(m!.ToLower(), StringComparison.Ordinal);
            
            Console.Write("Insert row: ");
            
            var n = Console.ReadLine()?.Trim();
            
            int.TryParse(n, out var y);
            try
            {
                _brain.RemoveShip( x, y-1, player);
            }
            catch (Exception e)
            {
                Console.WriteLine("invalid coordinates: " + m + "" + y);
                Console.Read();
            }
        }

        private ShipConfig chooseShip(int player)
        {
            var names = new Dictionary<string, ShipConfig>();
            foreach (var ship in _brain.getConfig().ShipConfigs)
            {
                if (ship.Quantity - _brain.GetPlayersShips(player).Count(ship1 => ship1.Config.Name == ship.Name) > 0)
                {
                    names.Add(ship.Name, ship);
                }
            }

            var selected = _brain.getConfig().ShipConfigs[0];
            
            if (names.Values.Count != 0)
            {
                selected = names.Values.ToArray()[ConsoleNavigator.MultipleChoiceWithTitle(false, "Ship types", names.Keys.ToArray())];
            }

            return selected;
        }

        private string chooseDirection()
        {
            var options = new string[]
            {
                "up", "down", "left", "right"
            };
            var directionDesignators = new string[] {"-y", "+y", "-x", "+x"};
            return directionDesignators[ConsoleNavigator.MultipleChoiceWithTitle(false, "Directions", options)];
        }
        

        private string BombingView1()
        {
            Console.Clear();
            var negResults = new List<string> {"SP"};
            negResults.AddRange(_menuSpecialShortcuts);
            var result = Bombing(0);
            while (!negResults.Contains(result))
            {
                Console.Clear();
                Console.WriteLine("You can try again!");
                result = Bombing(0);
            }
            
            var n = Console.ReadLine()?.Trim();
            if (_menuSpecialShortcuts.Contains(n!.ToUpper())) { return n.ToUpper(); }
            
            return result;
        }
        
        
        private string BombingView2()
        {
            Console.Clear();
            var negResults = new List<string>() {"SP"};
            negResults.AddRange(_menuSpecialShortcuts);
            var result = Bombing(1);
            while (!negResults.Contains(result))
            {
                Console.WriteLine("You can try again!");
                result = Bombing(1);
            }
            
            var n = Console.ReadLine()?.Trim();
            if (_menuSpecialShortcuts.Contains(n!.ToUpper())) { return n.ToUpper(); }
            
            return result;
        }

        private string Bombing(int playerId)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            
            BSConsoleUI.DrawBoard(_brain.getBoard(1, playerId));
            
            Console.Write("Insert column to bomb: ");
            
            var m = Console.ReadLine()?.Trim();

            var column = alphabet.IndexOf(m!.ToLower(), StringComparison.Ordinal);
            
            Console.Write("Insert row to bomb: ");
            
            var n = Console.ReadLine()?.Trim();
            
            if (_menuSpecialShortcuts.Contains(n!.ToUpper())) { return n.ToUpper(); }
            
            int.TryParse(n, out var row);

            var bombingResult = _brain.getBoard(1, playerId);
            try
            {
                bombingResult = _brain.bombBoardAt(Math.Abs(playerId - 1), column, row - 1);
            }
            catch (RuntimeBinderException e)
            {
                Console.WriteLine("Already bombed, try again");
                return "Retry";
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("invalid coordinates: " + m + "" + row);
                return "Retry";
            }
            
            Console.Clear();
            
            BSConsoleUI.DrawBoard(bombingResult);
            if (bombingResult[column, row - 1].Equals(new BoardSquareState(false, true)))
            {
                Console.WriteLine("Hit!");
                return "Retry";
            }

            Console.WriteLine("Miss!");
            
            if (Functions.useDB())
            {
                var id = 0;
                if (int.TryParse(_id, out id))
                {
                    DataHandler.UpdateSaveGameDB(_brain, id);
                }
                else
                {
                    _id = DataHandler.SaveGameToDB(_brain).ToString();
                }
            }
            else
            {
                var id = 0;
                if (int.TryParse(_id, out id))
                {
                    DataHandler.SaveGameToJson(_brain, Functions.GetGameJsonPath(), id+".json");
                    _id += ".json";
                }
                else
                {
                    DataHandler.SaveGameToJsonOverwrite(_brain, Functions.GetGameJsonPath(), _id);
                }
            }

            return "SP";
        }
        
        private string BoardViewPlayer1()
        {
            Console.Clear();
            BSConsoleUI.DrawBoard(_brain.getBoard(0, 0));
            
            var n = Console.ReadLine()?.Trim();
            if (_menuSpecialShortcuts.Contains(n!.ToUpper())) { return n.ToUpper(); }

            return "";
        }

        private string AddConf()
        {
            menuNotImplemented();
            return "";
        }
        
        private string BoardViewPlayer2()
        {
            Console.Clear();
            BSConsoleUI.DrawBoard(_brain.getBoard(0, 1));
            
            var n = Console.ReadLine()?.Trim();
            if (_menuSpecialShortcuts.Contains(n!.ToUpper())) { return n.ToUpper(); }

            return "";
        }

        public string LoadMenu()
        {
            if (!Functions.useDB())
            {
                var savesDictionary = DataHandler.LoadGameNamesFilenamesJson(Functions.GetGameJsonPath());

                var confNames = savesDictionary.Keys;

                string[] options = new string[savesDictionary.Keys.Count + 1];
                options[savesDictionary.Keys.Count] = "Return";
            
                savesDictionary.Keys.CopyTo(options, 0);

                var selected = ConsoleNavigator.MultipleChoiceWithTitle(false, "Load Game", options);

                if (options[selected] == "Return")
                {
                    return "";
                }

                _id = savesDictionary[options[selected]];
                _brain = DataHandler.ReadSaveGame(Functions.GetGameJsonPath(), savesDictionary[options[selected]]);
            }
            else
            {
                var saveNames = DataHandler.ReadSaveNamesAndIdsStringDB();
                var selected = ConsoleNavigator.MultipleChoiceWithTitle(true, "Load Game", saveNames.Keys.ToArray());

                if (selected == -1)
                {
                    return "";
                }

                _id = saveNames[saveNames.Keys.ToArray()[selected]];
                _brain = DataHandler.ReadSaveGameDB(int.Parse(saveNames[saveNames.Keys.ToArray()[selected]]));
            }
            
            ActivateGame();
            return "";
        }
        
        public string SaveGame()
        {
            if (Functions.useDB())
            {
                var id = 0;
                if (int.TryParse(_id, out id))
                {
                    DataHandler.UpdateSaveGameDB(_brain, id);
                }
                else
                {
                    _id = DataHandler.SaveGameToDB(_brain).ToString();
                }
            }
            else
            {
                var id = 0;
                if (int.TryParse(_id, out id))
                {
                    DataHandler.SaveGameToJson(_brain, Functions.GetGameJsonPath(), id+".json");
                    _id += ".json";
                }
                else
                {
                    DataHandler.SaveGameToJsonOverwrite(_brain, Functions.GetGameJsonPath(), _id);
                }
            }
            var options = new string[]
            {
                "Keep name " + _brain.getName(),
                "Rename"
            };
            var choice = ConsoleNavigator.MultipleChoiceWithTitle(false, "Save game", options);
            if (choice == 1)
            {
                Console.Write("Name your save: ");
                var gamename = Console.ReadLine()!.Trim().ToLower();
                if (Functions.useDB())
                {
                    DataHandler.UpdateSaveGameNameDB(gamename, int.Parse(_id));
                }
                else
                {
                    DataHandler.UpdateSaveGameNameJson(gamename, Functions.GetGameJsonPath(), _id);
                }
            }
            

            return "";
        }

        private string ConfMenu()
        {
            var menu = new Menu(EMenuLevel.primary, "Configurations");
            menu.AddMenuItems(new []
            {
                new MenuItem("V", "View existing configurations", ConfView),
                new MenuItem("N", "Create new config", NewConfView),
                new MenuItem("C", "Edit Configurations", EditConfView)
            });
            return menu.Run();
        }

        private string ConfView()
        {
            do
            {
                if (!Functions.useDB())
                {
                    _configs = LoadExistingConfigs();
                    var configs = GetConfigNameDictionary();
    
                    string[] options = new string[configs.Count + 1];
                    options[configs.Count] = "Return";
                
                    configs.Values.CopyTo(options, 0);
    
                    var selected = ConsoleNavigator.MultipleChoiceWithTitle(false, "Configurations", options);
    
                    if (options[selected] == "Return")
                    {
                        return "";
                    }
                    
                    Console.Clear();
    
                    Console.WriteLine(DataHandler.ReadConfigFromJsonFile(Functions.GetConfigurationJsonPath(),configs.Keys.ToArray()[selected]));
                    Console.Read();
                }
                else
                {
                    var configs = GetConfigNameDictionary();
                    var options = configs.Values.ToArray();
                    var selected = ConsoleNavigator.MultipleChoiceWithTitle(true, "Configurations", options);
                    if (selected == -1)
                    {
                        return "";
                    }

                    Console.Clear();

                    Console.WriteLine(DataHandler.ReadGameCofigDB(int.Parse(configs.Keys.ToArray()[selected])));
                    Console.Read();
                }
                
            } while (true);
        }
        
        
        private string NewConfView()
        {
            GameConfig gameConfig = new();

            string[] options = new string[gameConfig.Fields() + 1];
            options[gameConfig.Fields()] = "Return";
             
            gameConfig.FieldNames().CopyTo(options, 0);
            do
            {
                var selected = ConsoleNavigator.MultipleChoiceWithTitle(false, "New Config", options);

                switch (options[selected])
                {
                    case "Name":
                        Console.Clear();
                        Console.Write("Name your config: ");
                        var name = Console.ReadLine()?.Trim() ?? "";
                        gameConfig.Name = name;
                        Console.WriteLine(gameConfig);
                        Console.Read();
                        break;
                    case "Board height":
                        Console.Clear();
                        Console.Write("Set board height: ");
                        var ysize = Console.ReadLine()?.Trim() ?? "";
                        int.TryParse(ysize, out var ysizenum);
                        gameConfig.BoardHeight = ysizenum;
                        break;
                    case "Board width":
                        Console.Clear();
                        Console.Write("Set board width: ");
                        var xsize = Console.ReadLine()?.Trim() ?? "";
                        int.TryParse(xsize, out var xsizenum);
                        gameConfig.BoardWidth = xsizenum != 0 ? xsizenum : 10;
                        break;
                    case "Ship placement":
                        var values = new Dictionary<string, EShipPlacement>();
                        values["sides can touch"] = EShipPlacement.SideTouch;
                        values["can touch diagonally"] = EShipPlacement.CornerTouch;
                        values["ships cant touch"] = EShipPlacement.NoTouch;
                        var ops = new []
                        {
                            "sides can touch",
                            "can touch diagonally",
                            "ships cant touch"
                        };
                        gameConfig.ShipPlacement = values[ops[ConsoleNavigator.MultipleChoiceWithTitle(false, "Ship placement", ops)]];
                        Console.WriteLine(gameConfig);
                        Console.Read();
                        break;
                    case "Return":
                        if (!Functions.useDB())
                        {
                            var valuePair = Functions.newConf();
                            DataHandler.SaveConfigToJsonOverwrite(gameConfig, Functions.GetConfigurationJsonPath(),  valuePair.Value);
                        }
                        else
                        {
                            DataHandler.SaveConfigToDB(gameConfig);
                        }
                        
                        Console.WriteLine(gameConfig);
                        Console.Read();
                        return "";
                    default:
                        Console.WriteLine("I dumb");
                        Console.Read();
                        break;
                }
            } while (true);
        }

        private string EditConfView()
        {
            var gameConfig = _config;
            var id = "0";
            if (!Functions.useDB())
            {
                _configs = LoadExistingConfigs();
                var configs = GetConfigNameDictionary();

                string[] options = new string[configs.Count + 1];
                options[configs.Count] = "Return";
            
                configs.Values.CopyTo(options, 0);

                var selected = ConsoleNavigator.MultipleChoiceWithTitle(false, "Edit configurations", options);

                if (options[selected] == "Return")
                {
                    return "";
                }
                
                Console.Clear();
                id = configs.Keys.ToArray()[selected];
                gameConfig = DataHandler.ReadConfigFromJsonFile(Functions.GetConfigurationJsonPath(),id);
            }
            else
            {
                var configs = GetConfigNameDictionary();
                var options = configs.Values.ToArray();
                var selected = ConsoleNavigator.MultipleChoiceWithTitle(true, "Configurations", options);
                if (selected == -1)
                {
                    return "";
                }

                Console.Clear();

                id = configs.Keys.ToArray()[selected];
                gameConfig = DataHandler.ReadGameCofigDB(int.Parse(id));
            }
            var selections = new string[gameConfig.Fields() + 1];
            selections[gameConfig.Fields()] = "Return";
            
            gameConfig.FieldNames().CopyTo(selections, 0);
            do
            {
                var selected = ConsoleNavigator.MultipleChoiceWithTitle(false, gameConfig.Name, selections);

                switch (selections[selected])
                {
                    case "Name":
                        Console.Clear();
                        Console.WriteLine("Current: " + gameConfig.Name);
                        Console.Write("Name your config: ");
                        var name = Console.ReadLine()?.Trim() ?? "";
                        gameConfig.Name = name == "" ? gameConfig.Name : name;
                        Console.WriteLine(gameConfig);
                        Console.Read();
                        break;
                    case "Board height":
                        Console.Clear();
                        Console.WriteLine("Current: " + gameConfig.BoardHeight);
                        Console.Write("Set board height: ");
                        var ysize = Console.ReadLine()?.Trim() ?? "";
                        ysize = ysize == "" ? gameConfig.BoardHeight.ToString() : ysize;
                        int.TryParse(ysize, out var ysizenum);
                        gameConfig.BoardHeight = ysizenum;
                        break;
                    case "Board width":
                        Console.Clear();
                        Console.WriteLine("Current: " + gameConfig.BoardWidth);
                        Console.Write("Set board width: ");
                        var xsize = Console.ReadLine()?.Trim() ?? "";
                        xsize = xsize == "" ? gameConfig.BoardWidth.ToString() : xsize;
                        int.TryParse(xsize, out var xsizenum);
                        gameConfig.BoardWidth = xsizenum != 0 ? xsizenum : 10;
                        break;
                    case "Ship placement":
                        var values = new Dictionary<string, EShipPlacement>();
                        values["sides can touch"] = EShipPlacement.SideTouch;
                        values["can touch diagonally"] = EShipPlacement.CornerTouch;
                        values["ships cant touch"] = EShipPlacement.NoTouch;
                        var ops = new []
                        {
                            "sides can touch",
                            "can touch diagonally",
                            "ships cant touch"
                        };
                        gameConfig.ShipPlacement = values[ops[ConsoleNavigator.MultipleChoiceWithTitle(false, "Placement", ops)]];
                        Console.WriteLine(gameConfig);
                        Console.Read();
                        break;
                    case "Return":
                        if (!Functions.useDB())
                        {
                            DataHandler.SaveConfigToJsonOverwrite(gameConfig, Functions.GetConfigurationJsonPath(),  id);
                        }
                        else
                        {
                            DataHandler.UpdateConfigDB(gameConfig);
                        }
                        
                        Console.WriteLine(gameConfig);
                        Console.Read();
                        return "";
                    default:
                        Console.WriteLine("I dumb");
                        Console.Read();
                        break;
                }
            } while (true);
        }

        public string NewGame()
        {
            Console.Clear();

            var newGameMenu = new Menu(EMenuLevel.primary, "New game");
            
            newGameMenu.AddMenuItems(new []
            {
                new MenuItem("S", "Standard game", NewStandardGame),
                new MenuItem("C", "Custom game", NewCustomGame)
            });

            return newGameMenu.Run();
            
            Console.Write("Insert board size: ");
            var n = Console.ReadLine()?.Trim();
            
            if (_menuSpecialShortcuts.Contains(n!.ToUpper())) { return n.ToUpper(); }
            
            int.TryParse(n, out var converted);

            _brain = new BSBrain(converted, converted);
            _gameActive = true;

            return CurrentGame();   
        }

        private string NewStandardGame()
        {
            Console.WriteLine(_configs.Count);
            if (Functions.useDB())
            {
                _id = Functions.start(1).ToString();
                _brain = DataHandler.ReadSaveGameDB(int.Parse(_id));
            }
            else
            {
                _id = Functions.startjson("standard.json");
                _brain = DataHandler.ReadSaveGame(Functions.GetGameJsonPath(), _id);
            }
            ActivateGame();
            return CurrentGame();
        }


        private string NewCustomGame()
        {
            var confs = GetConfigNameDictionary();

            var options = confs.Values.ToArray();
            
            var selected = ConsoleNavigator.MultipleChoiceWithTitle(false, "Configurations", options);

             var id = "";
             if (Functions.useDB())
             {
                  id = Functions.start(int.Parse(confs.Keys.ToArray()[selected])).ToString();
                  _brain = DataHandler.ReadSaveGameDB(int.Parse(id));
                  _id = id;
             }
             else
             {
                 id = Functions.startjson(confs.Keys.ToArray()[selected]);
                 _brain = DataHandler.ReadSaveGame(Functions.GetGameJsonPath(), id);
                 _id = id;
             }
             
            ActivateGame();
            return CurrentGame();
        }

        private void ActivateGame()
        {
            try
            {
                _mainmenu.AddMenuItem(new MenuItem("C", "Current game", CurrentGame), 0);
                _mainmenu.AddMenuItem(new MenuItem("S", "Save game", SaveGame), 0);
                _mainmenu.AddMenuItem(new MenuItem("H", "History", History), 4);
                _gameActive = true;
            }
            catch (Exception e)
            {
                
            }
        }

        private string SettingsMenu()
        {
            while (true)
            {
               string[] options = new[]
                           {
                               Functions.useDB() ? "Set saves to local storage" : "Set saves to database",
                               "Return"
                           };
                var selected = ConsoleNavigator.MultipleChoiceWithTitle(false, "Settings", options);
                switch (selected)
                {
                    case 0:
                        Functions.switchSaving();
                        _saveLocaly = !_saveLocaly;
                            break;
                    case 1:
                        return "";
                } 
            }
            
        }

        private string History()
        {
            ConsoleKey key;
            Console.CursorVisible = false;

            var currentSelection = 1;

            do
            {
                var options = new []
                {
                    "To the beginning",
                    "Undo",
                    "Redo",
                    "To the end"
                };
                if (_brain.GetHistory().GetUndoStackSize() == 0)
                {
                    options = new []
                    {
                        "Redo",
                        "To the end"
                    };
                }
                else if (_brain.GetHistory().GetRedoStackSize() == 0)
                {
                    options = new []
                    {
                        "To the beginning",
                        "Undo"
                    };
                }

                if (currentSelection >= options.Length) currentSelection = 1;

                Console.Clear();
                
                BSConsoleUI.DrawBoardsSideBySide(_brain.getBoard(0, 0), _brain.getBoard(0, 1));

                for (int i = 0; i < options.Length; i++)
                {
                    var atm = Console.ForegroundColor;

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write(options[i]);
                    Console.Write("    ");

                    Console.ForegroundColor = atm;
                }
                
                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Escape:
                        if (Functions.useDB())
                        {
                            var id = 0;
                            if (int.TryParse(_id, out id))
                            {
                                DataHandler.UpdateSaveGameDB(_brain, id);
                            }
                            else
                            {
                                _id = DataHandler.SaveGameToDB(_brain).ToString();
                            }
                        }
                        else
                        {
                            var id = 0;
                            if (int.TryParse(_id, out id))
                            {
                                DataHandler.SaveGameToJson(_brain, Functions.GetGameJsonPath(), id+".json");
                                _id += ".json";
                            }
                            else
                            {
                                DataHandler.SaveGameToJsonOverwrite(_brain, Functions.GetGameJsonPath(), _id);
                            }
                        }
                        Console.CursorVisible = true;
                        return "";
                    case ConsoleKey.LeftArrow:
                        if (currentSelection >= 1)
                        {
                            currentSelection -= 1;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentSelection + 1 < options.Length)
                        {
                            currentSelection += 1;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (options[currentSelection])
                        {
                            case "To the beginning":
                                _brain.ToBeginning();
                                break;
                            case "Undo":
                                _brain.Undo();
                                break;
                            case "Redo":
                                _brain.Redo();
                                break;
                            case "To the end":
                                _brain.Continue();
                                break;
                        }

                        break;
                }
                
                
            } while (true);
            return "";
        }

        public string menuNotImplemented()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Not implemeted yet :(");
            Console.ResetColor();
            return "";
        }
    }
}