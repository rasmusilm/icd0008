using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using BattleshipBasicTypes;
using DAL;
using Domain;

namespace BattleshipBrain
{
    public class DataHandler
    {
        
        public static void SaveConfigToJson(GameConfig conf, string filepath, string fileName = "standard.json")
        {
            var confStr = JsonSerializer.Serialize(conf);
            File.WriteAllText(Path.Join(filepath, fileName), confStr);
        }
        
        public static void SaveConfigToJsonOverwrite(GameConfig conf, string filepath, string fileName = "standard.json")
        {
            var confStr = JsonSerializer.Serialize(conf);
            if (File.Exists(Path.Join(filepath, fileName)))
            {
                File.Delete(Path.Join(filepath, fileName));
            }
            File.WriteAllText(Path.Join(filepath, fileName), confStr);
        }
        
        public static void SaveConfigToJsonOverwrite(string confStr, string filepath, string fileName = "standard.json")
        {
            Console.WriteLine(fileName);
            if (File.Exists(Path.Join(filepath, fileName)))
            {
                Console.WriteLine("Deleting");
                File.Delete(Path.Join(filepath, fileName));
            }
            Console.WriteLine(confStr);
            File.WriteAllText(Path.Join(filepath, fileName), confStr);
        }

        public static GameConfig ReadConfigFromJsonFile(string dirpath, string FileName)
        {
            Console.WriteLine(dirpath);
            GameConfig? read = null;
            
            if (File.Exists(Path.Join(dirpath, FileName)))
            {
                var readString = File.ReadAllText(Path.Join(dirpath, FileName));
                Console.WriteLine(readString);
                read = JsonSerializer.Deserialize<GameConfig>(readString);
            }

            var config = read!;
            Console.WriteLine(config.Name);
            Console.WriteLine(JsonSerializer.Serialize(config));
            return config;
        }

        public static GameConfig ReadConfigFromJsonFile(string path)
        {
            object? read = null;
            
            if (File.Exists(path))
            {
                var readString = File.ReadAllText(path);
                read = JsonSerializer.Deserialize(readString, typeof(GameConfig));
            }

            var config = (GameConfig) read!;
            Console.WriteLine(config.Name);
            return config;
        }

        public static GameConfig ReadConfigFromJson(string json)
        {
            var read = JsonSerializer.Deserialize(json, typeof(GameConfig));

            var config = (GameConfig) read!;
            Console.WriteLine(config.Name);
            return config;
        }

        public static void SaveGameToJson(BSBrain brain, string filepath, string filename)
        {
            var gamestateJson = brain.GetBrainDto();
            File.WriteAllText(Path.Join(filepath, filename), JsonSerializer.Serialize(gamestateJson));
        }
        
        public static void SaveGameToJsonOverwrite(BSBrain brain, string filepath, string filename)
        {
            var gamestateJson = brain.GetBrainDto();
            if (File.Exists(Path.Join(filepath, filename)))
            {
                File.Delete(Path.Join(filepath, filename));
            }
            File.WriteAllText(Path.Join(filepath, filename), JsonSerializer.Serialize(gamestateJson));
        }

        public BSBrain ReadPlayersAsSaveGame(string dirpath, string filename)
        {
            object? read = null;
            object? read2 = null;
            
            var serializer = new JsonSerializerOptions();
            
            if (File.Exists(Path.Join(dirpath, filename)))
            {
                var readString = File.ReadAllText(Path.Join(dirpath, filename));
                var info = readString.Split("\nB: ");
                read = JsonSerializer.Deserialize(info[0], typeof(PlayerDTO), serializer);
                read2 = JsonSerializer.Deserialize(info[1], typeof(PlayerDTO), serializer);
            }

            var player1 = Player.deserialize((PlayerDTO) read!);
            var player2 = Player.deserialize((PlayerDTO) read2!);

            return BSBrain.RegnenerateBrain(player1, player2);
        }

        public static BSBrain ReadSaveGame(string dirpath, string filename)
        {
            BrainDTO newbrain = null!;
            if (File.Exists(Path.Join(dirpath, filename)))
            {
                var readString = File.ReadAllText(Path.Join(dirpath, filename));
                var regen = JsonSerializer.Deserialize(readString, typeof(BrainDTO));
                newbrain = (BrainDTO) regen!;
            }
            return BSBrain.RegnenerateBrain(newbrain);
        }
        
        public static BSBrain ReadSaveGame(string path)
        {
            Console.WriteLine(path);
            BrainDTO newbrain = null!;
            if (File.Exists(path))
            {
                var readString = File.ReadAllText(path);
                var regen = JsonSerializer.Deserialize(readString, typeof(BrainDTO));
                newbrain = (BrainDTO) regen!;
            }

            Console.WriteLine(newbrain.player1);
            return BSBrain.RegnenerateBrain(newbrain);
        }
        
        public static string GetGameNameJson(string dirpath, string filename)
        {
            BrainDTO newbrain = null!;
            if (File.Exists(Path.Join(dirpath, filename)))
            {
                var readString = File.ReadAllText(Path.Join(dirpath, filename));
                var regen = JsonSerializer.Deserialize(readString, typeof(BrainDTO));
                newbrain = (BrainDTO) regen!;
            }

            return newbrain.Name;
        }
        
        public static string UpdateSaveGameNameJson(string name, string dirpath, string filename)
        {
            BrainDTO newbrain = null!;
            if (File.Exists(Path.Join(dirpath, filename)))
            {
                var readString = File.ReadAllText(Path.Join(dirpath, filename));
                var regen = JsonSerializer.Deserialize(readString, typeof(BrainDTO));
                newbrain = (BrainDTO) regen!;
            }

            newbrain.Name = name;
            
            if (File.Exists(Path.Join(dirpath, filename)))
            {
                File.Delete(Path.Join(dirpath, filename));
            }
            File.WriteAllText(Path.Join(dirpath, filename), JsonSerializer.Serialize(newbrain));

            return newbrain.Name;
        }
        
        public static List<GameConfig> LoadExistingConfigs(string ConfigPath)
        {
            List<GameConfig> loaded = new();
            string[] files = Directory.GetFiles(ConfigPath);
            int counter = 1;
            foreach (var file in files)
            {
                var conf = ReadConfigFromJsonFile(file);
                loaded.Add(conf);
            }
            return loaded;
        }
        
        public static int UpdateSaveGameDB(BSBrain brain, int id)
        {
            using var db = new BSDBContext();

            var players = brain.GetPlayerDbDtos();

            var dbPlayers = db.Players.Where(P => P.BrainDbDTOId == id).ToArray();
            
            var player1 = dbPlayers[0];
            player1.Ships = players[0].Ships;
            player1.EnemyBoard = players[0].EnemyBoard;
            player1.MyBoard = players[0].MyBoard;
            var player2 = dbPlayers[1];
            player2.Ships = players[1].Ships;
            player2.EnemyBoard = players[1].EnemyBoard;
            player2.MyBoard = players[1].MyBoard;

            var history = brain.GetHistory().GetDto();

            var dbHistory = db.History.FirstOrDefault(h => h.BrainDbDTOId == id)!;

            dbHistory.Redo = history.Redo;
            dbHistory.Undo = history.Undo;

            db.SaveChanges();
            
            return id;
        }

        public static string GetGameNameDB(int id)
        {
            using var db = new BSDBContext();

            var game = db.Games.FirstOrDefault(g => g.BrainDbDTOId == id);

            return game!.GameName!;
        }
        
        public static int UpdateSaveGameNameDB(string name, int id)
        {
            using var db = new BSDBContext();

            var game = db.Games.FirstOrDefault(g => g.BrainDbDTOId == id);

            game!.GameName = name;
            
            db.SaveChanges();
            
            return id;
        }
        
        public static int SaveGameToDB(BSBrain brain)
        {
            using var db = new BSDBContext();
            var game = brain.GetBrainDbDto();
            var id = db.Games.Count();
            db.Games.Add(game);

            db.SaveChanges();

            var gameSaved = db.Games.OrderBy(g => g.BrainDbDTOId).Last();

            var players = brain.GetPlayerDbDtos();

            foreach (var player in players) player.BrainDbDTOId = gameSaved.BrainDbDTOId;
            
            db.Players.Add(players[0]);
            db.Players.Add(players[1]);

            var history = brain.GetHistory().GetDto();

            history.BrainDbDTOId = gameSaved.BrainDbDTOId;

            db.History.Add(history);
            
            db.SaveChanges();
            
            return id;
        }
        
        public static int SaveGameToDB(BSBrain brain, string name)
        {
            using var db = new BSDBContext();
            var game = brain.GetBrainDbDto();
            game.GameName = name;
            var id = db.Games.Count();
            db.Games.Add(game);

            db.SaveChanges();

            var gameSaved = db.Games.OrderBy(g => g.BrainDbDTOId).Last();

            var players = brain.GetPlayerDbDtos();

            foreach (var player in players) player.BrainDbDTOId = gameSaved.BrainDbDTOId;
            
            db.Players.Add(players[0]);
            db.Players.Add(players[1]);
            
            var history = brain.GetHistory().GetDto();

            history.BrainDbDTOId = gameSaved.BrainDbDTOId;

            db.History.Add(history);
            
            db.SaveChanges();
            
            return gameSaved.BrainDbDTOId;
        }

        public static string[] ReadSaveNamesDB()
        {
            using var db = new BSDBContext();
            var games = new List<string>();
            foreach (var game in db.Games)
            {
                games.Add(game.GameName);
            }

            var names = games.ToArray();

            return names;
        }
        
        public static Dictionary<string, int> ReadSaveNamesAndIdsDB()
        {
            using var db = new BSDBContext();
            var games = new Dictionary<string, int>();
            foreach (var game in db.Games)
            {
                if (games.ContainsKey(game.GameName))
                {
                    games.Add(game.GameName + "_" + games.Count, game.BrainDbDTOId);
                }
                else
                {
                    games.Add(game.GameName, game.BrainDbDTOId);
                }
                
            }

            return games;
        }
        
        public static Dictionary<string, string> ReadSaveNamesAndIdsStringDB()
        {
            using var db = new BSDBContext();
            var games = new Dictionary<string, string>();
            foreach (var game in db.Games)
            {
                if (games.ContainsKey(game.GameName))
                {
                    games.Add(game.GameName + "_" + games.Count, game.BrainDbDTOId.ToString());
                }
                else
                {
                    games.Add(game.GameName, game.BrainDbDTOId.ToString());
                }
                
            }

            return games;
        }

        public static BSBrain ReadSaveGameDB(string name)
        {
            using var db = new BSDBContext();
            var game = db.Games.FirstOrDefault(g => g.GameName == name);
            var players = db.Players.Where(p => p.BrainDbDTOId == game!.BrainDbDTOId).ToArray();

            var brain = AcquireBrain(game!, players[0], players[1]);

            var history = db.History.FirstOrDefault(h => h.BrainDbDTOId == game!.BrainDbDTOId);
            
            brain.SepHistory(MoveHistory.getFromDto(history!));
            return brain;
        }
        
        public static BSBrain ReadSaveGameDB(int id)
        {
            using var db = new BSDBContext();
            var game = db.Games.FirstOrDefault(g => g.BrainDbDTOId == id);
            var players = db.Players.Where(p => p.BrainDbDTOId == game!.BrainDbDTOId).ToArray();

            var brain = AcquireBrain(game!, players[0], players[1]);

            var history = db.History.FirstOrDefault(h => h.BrainDbDTOId == game!.BrainDbDTOId);
            
            brain.SepHistory(MoveHistory.getFromDto(history!));
            return brain;
        }
        
        public static string[] ReadConfigNamesDB()
        {
            using var db = new BSDBContext();
            var configs = new List<string>();
            foreach (var conf in db.Configurations)
            {
                configs.Add(conf.Name);
            }

            var names = configs.ToArray();

            return names;
        }
        
        public static Dictionary<string, string> ReadConfigNamesIdDB()
        {
            using var db = new BSDBContext();
            var configs = new Dictionary<string, string>();
            foreach (var conf in db.Configurations)
            {
                configs.Add(conf.GameConfigDbDtoId.ToString(), conf.Name);
            }
            
            return configs;
        }
        
        public static GameConfig ReadGameCofigDB(string name)
        {
            using var db = new BSDBContext();
            var game = db.Configurations.FirstOrDefault(g => g.Name == name);

            return GameConfig.RegenerateConfig(game);
        }
        
        public static GameConfig ReadGameCofigDB(int id)
        {
            using var db = new BSDBContext();
            var config = db.Configurations.FirstOrDefault(g => g.GameConfigDbDtoId == id);

            return GameConfig.RegenerateConfig(config);
        }

        public static int SaveConfigToDB(GameConfig config)
        {
            using var db = new BSDBContext();
            var gameConfig = config.ToDto();
            var id = db.Configurations.Count();
            db.Configurations.Add(gameConfig);
            db.SaveChanges();

            return id;
        }

        public static void UpdateConfigDB(GameConfig config)
        {
            using var db = new BSDBContext();
            var gameConfig = config.ToDto();
            var conf = db.Configurations.FirstOrDefault(c => c.GameConfigDbDtoId == config.Id);
            conf!.Name = gameConfig.Name;
            conf.BoardHeight = gameConfig.BoardHeight;
            conf.BoardWidth = gameConfig.BoardHeight;
            conf.ShipConfigs = gameConfig.ShipConfigs;
            conf.ShipPlacement = gameConfig.ShipPlacement;
            db.SaveChanges();
        }

        public static BSBrain AcquireBrain(BrainDbDTO braintable, PlayerDbDTO playerDbDto1, PlayerDbDTO playerDbDto2)
        {
            var player1 = Player.regenerate(playerDbDto1);
            var player2 = Player.regenerate(playerDbDto2);
            return BSBrain.RegnenerateBrain(braintable, player1, player2);
        }

        public static string[] GetKeysFromDictionary<T>(Dictionary<string, T> dictionary)
        {
            var Names = dictionary.Keys;
            string[] options = new string[dictionary.Keys.Count];
            
            var counter = 0;
            foreach (var opt in Names)
            {
                string option = "";
                if (opt is string)
                {
                    option = opt;
                    options[counter] = option;
                    counter++;
                }
            }

            return options;
        }

        public static Dictionary<string, string> LoadConfigsNamesFilenamesJson(string path)
        {
            Dictionary<string, string> configNames = new Dictionary<string, string>();
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var conf = ReadConfigFromJsonFile(file);
                Console.WriteLine(file);
                var paths = file.Split("\\");
                configNames.Add(paths[^1], conf.Name);
            }

            return configNames;
        }
        
        public static Dictionary<string, string> LoadGameNamesFilenamesJson(string path)
        {
            Dictionary<string, string> GameNames = new ();
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var game = ReadSaveGame(file);
                Console.WriteLine(file);
                var paths = file.Split("\\");
                Console.WriteLine(game.getName());
                GameNames.Add(game.getName(), paths[^1]);
            }

            return GameNames;
        }

        public static void DeleteGame(int id)
        {
            using var db = new BSDBContext();
            db.Games.Remove(db.Games.FirstOrDefault(g => g.BrainDbDTOId == id)!);
            db.History.Remove(db.History.FirstOrDefault(h => h.BrainDbDTOId == id)!);
            db.SaveChanges();
        }
        
        public static void DeleteConfig(int id)
        {
            using var db = new BSDBContext();
            var affectedGames = db.Games.Where(g=> g.GameConfigDbDtoId == id).ToList();
            if (affectedGames.Count > 0)
            {
                foreach (var game in affectedGames)
                {
                    db.Games.Remove(game);
                }
            }
            db.Configurations.Remove(db.Configurations.FirstOrDefault(c=>c.GameConfigDbDtoId == id)!);
            db.SaveChanges();
        }

        public static void DeleteConfigJson(string path, string filename)
        {
            Console.WriteLine(filename);
            if (File.Exists(Path.Join(path, filename)))
            {
                Console.WriteLine("Deleting");
                File.Delete(Path.Join(path, filename));
            }
        }
        
        public static void DeleteGameJson(string path, string filename)
        {
            Console.WriteLine(filename);
            if (File.Exists(Path.Join(path, filename)))
            {
                Console.WriteLine("Deleting");
                File.Delete(Path.Join(path, filename));
            }
        }
    }
}