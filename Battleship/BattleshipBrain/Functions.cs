using System;
using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BattleshipBrain
{
    public class Functions
    {
        public static int start(string confname)
        {
            int id = 0;
            var config = DataHandler.ReadGameCofigDB(confname);
            var game = new BSBrain(config.BoardHeight, config.BoardWidth);
            id = DataHandler.SaveGameToDB(game, DateTime.Now.ToString("yyyyMMddHHmmss"));
            return id;
        }

        public static int start(int confId)
        {
            int id = 0;
            var config = DataHandler.ReadGameCofigDB(confId);
            var game = new BSBrain(config);
            id = DataHandler.SaveGameToDB(game, DateTime.Now.ToString("yyyyMMddHHmmss"));
            return id;
        }
        
        public static string startjson(string confname)
        {
            int id = 0;
            var config = DataHandler.ReadConfigFromJsonFile(GetConfigurationJsonPath(),confname);
            var game = new BSBrain(config);
            var name = DateTime.Now.ToString("yyyyMMddHHmmss");
            var filename = name + ".json";
            game.Rename(name);
            DataHandler.SaveGameToJson(game, GetGameJsonPath(), filename);
            return filename;
        }

        public static KeyValuePair<GameConfig, string> newConf()
        {
            var conf = new GameConfig();
            var name = DateTime.Now.ToString("yyyyMMddHHmmss")+".json";
            DataHandler.SaveConfigToJson(conf, GetConfigurationJsonPath(), name);
            return new KeyValuePair<GameConfig, string>(conf, name);
        }

        public static int load(string confname)
        {
            using var db = new BSDBContext();
            var id = db.Configurations.FirstOrDefault(c => c.Name == confname)!.GameConfigDbDtoId;
            return id;
        }

        public static bool useDB()
        {
            using var db = new BSDBContext();
            var answer = db.Settings.FirstOrDefault(s => s.SettingsID == 1)!.saveToDB;
            return answer;
        }
        
        public static void switchSaving()
        {
            using var db = new BSDBContext();
            var answer = db.Settings.FirstOrDefault(s => s.SettingsID == 1);
            answer!.saveToDB = !answer.saveToDB;
            db.SaveChanges();
        }

        public static string GetGameJsonPath()
        {
            return "C:/Users/rasmu/icd0008-2021f/Battleship/BattleShipConsoleApp/Savegames";
        }

        public static string GetConfigurationJsonPath()
        {
            return "C:/Users/rasmu/icd0008-2021f/Battleship/BattleShipConsoleApp/Configs";
        }
    }
}