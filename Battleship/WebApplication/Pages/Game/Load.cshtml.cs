using System;
using System.Collections.Generic;
using BattleshipBrain;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class Load : PageModel
    {
        public Dictionary<string, string> Saves = new();
        public string Name = "";
        public string Id = "0";
        public bool ChangeName = false;

        public void OnGet(string cmd, string id, string name)
        {
            if (Functions.useDB())
            {
                if (cmd == "name")
                {
                    Name = DataHandler.GetGameNameDB(int.Parse(id));
                    ChangeName = true;
                    Id = id;
                }
                else
                {
                    if (cmd == "del")
                    {
                        DataHandler.DeleteGame(int.Parse(id));
                    }
                    else if (cmd == "rename")
                    {
                        DataHandler.UpdateSaveGameNameDB(name, int.Parse(id));
                        var game = DataHandler.ReadSaveGameDB(int.Parse(id));
                        Console.WriteLine(game.getName());
                    }
                    Saves = DataHandler.ReadSaveNamesAndIdsStringDB();
                }
                
            }
            else
            {
                if (cmd == "name")
                {
                    Name = DataHandler.GetGameNameJson(Functions.GetGameJsonPath(), id);
                    ChangeName = true;
                    Id = id;
                }
                else
                {
                    if (cmd == "del")
                    {
                        DataHandler.DeleteGameJson(Functions.GetGameJsonPath(), id);
                    }
                    else if (cmd == "rename")
                    {
                        DataHandler.UpdateSaveGameNameJson(name, Functions.GetGameJsonPath(), id);
                    }
                }
                
                Saves = DataHandler.LoadGameNamesFilenamesJson(Functions.GetGameJsonPath());
            }
            
        }
    }
}