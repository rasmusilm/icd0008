using System.Collections.Generic;
using System.IO;
using System.Linq;
using BattleshipBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class start_new : PageModel
    {
        public string[] configs = new string[2];
        
        public void OnGet()
        {
            if (Functions.useDB())
            {
                configs = DataHandler.ReadConfigNamesDB();
            }
            else
            {
                configs = GetConfigNameDictionary().Keys.ToArray();
            }
            
        }
        
        private List<GameConfig> LoadExistingConfigs()
        {
            List<GameConfig> loaded = new();
            string[] files = Directory.GetFiles("C:/Users/rasmu/icd0008-2021f/Battleship/BattleShipConsoleApp/Configs");
            int counter = 1;
            foreach (var file in files)
            {
                var conf = DataHandler.ReadConfigFromJsonFile(file);
                loaded.Add(conf);
            }
            return loaded;
        }

        private Dictionary<string, GameConfig> GetConfigNameDictionary()
        {
            var _configs = LoadExistingConfigs();
            Dictionary<string, GameConfig> configs_by_name = new ();
            foreach (var conf in _configs) {configs_by_name.Add(conf.Name, conf);}

            return configs_by_name;
        }
    }
}