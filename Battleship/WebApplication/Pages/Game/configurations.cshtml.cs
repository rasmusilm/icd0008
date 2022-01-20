using System;
using System.Collections.Generic;
using System.Text.Json;
using BattleshipBrain;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class configurations : PageModel
    {
        public bool view = false;
        private readonly BSDBContext _context = new ();
        public Dictionary<string, string> Configs = new ();
        public string cmd = "";
        public string Message = "";
        public string ID = "";
        public bool useDB = Functions.useDB();
        public GameConfig? ToEdit { get; set; }
        
        public void OnGet(string cmd, string id, string save)
        {
            if (Functions.useDB())
            {
                switch (cmd)
                {
                    case "edit":
                        this.cmd = cmd;
                        ToEdit = GameConfig.RegenerateConfig(_context.Configurations.Find(int.Parse(id)));
                        break;
                    case "view":
                        this.cmd = cmd;
                        ToEdit = DataHandler.ReadGameCofigDB(int.Parse(id));
                        break;
                    case "save":
                        Console.WriteLine(save);
                        ToEdit = DataHandler.ReadConfigFromJson(save);
                        DataHandler.UpdateConfigDB(ToEdit);
                        break;
                    case "new":
                        this.cmd = "edit";
                        ToEdit = new GameConfig();
                        ToEdit.Id = DataHandler.SaveConfigToDB(ToEdit);
                        break;
                    case "del":
                        if (id != "1")
                        {
                            DataHandler.DeleteConfig(int.Parse(id));
                        }
                        
                        break;
                }
                Configs = DataHandler.ReadConfigNamesIdDB();
            }
            else
            {
                
                switch (cmd)
                {
                    case "edit":
                        this.cmd = cmd;
                        ToEdit = DataHandler.ReadConfigFromJsonFile(Functions.GetConfigurationJsonPath(), id);
                        Console.WriteLine(ToEdit.ShipConfigs[0].Quantity);
                        Console.WriteLine(JsonSerializer.Serialize(ToEdit));
                        ID = id;
                        break;
                    case "view":
                        this.cmd = cmd;
                        ToEdit = DataHandler.ReadConfigFromJsonFile(Functions.GetConfigurationJsonPath(), id);
                        ID = id;
                        break;
                    case "save":
                        Console.WriteLine(save);
                        Console.WriteLine(id);
                        ToEdit = DataHandler.ReadConfigFromJson(save);
                        DataHandler.SaveConfigToJsonOverwrite(save, Functions.GetConfigurationJsonPath(), id);
                        break;
                    case "new":
                        this.cmd = "edit";
                        var NewConf = Functions.newConf();
                        ToEdit = NewConf.Key;
                        ID = NewConf.Value;
                        break;
                    case "del":
                        if (id != "standard.json")
                        {
                            DataHandler.DeleteConfigJson(Functions.GetConfigurationJsonPath(), id);
                        }
                        break;
                }
                Configs = DataHandler.LoadConfigsNamesFilenamesJson(Functions.GetConfigurationJsonPath());
            }
            
        }
        
        public JsonResult OnPostTest([FromBody]string json)
        {
            Console.WriteLine(json);
            return new JsonResult(json);
        }
    }
}