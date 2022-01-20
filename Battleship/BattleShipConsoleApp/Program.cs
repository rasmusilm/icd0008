using System;
using System.Text.Json;
using System.IO;
using System.Linq;
using BattleshipBasicTypes;
using BattleshipBrain;
using DAL;
using Microsoft.EntityFrameworkCore.Design;

namespace BattleShipConsoleApp
{
    class BSConsoleApp
    {
        static void Main(string[] args)
        {
            string _basePath;
            Console.WriteLine("Battleship");
            
            _basePath = args.Length == 1 ? args[0] : Directory.GetCurrentDirectory();
            

            string confPath = _basePath + Path.DirectorySeparatorChar + "BattleShipConsoleApp"+ Path.DirectorySeparatorChar + "Configs";
            Console.WriteLine(confPath);

            var standardConf = DataHandler.ReadConfigFromJsonFile(confPath, "standard.json");

            Console.WriteLine(standardConf.Name);

            Application AppInstance = new(standardConf);
            AppInstance.ConfigPath = confPath;
            AppInstance.SaveGamePath = _basePath + Path.DirectorySeparatorChar + "BattleShipConsoleApp" +
                                       Path.DirectorySeparatorChar + "Savegames";

            AppInstance.Run();
            
            Console.WriteLine("Hello World!");

            using var db = new BSDBContext();
            Console.WriteLine("Person count " + db.Games.Count());
        }
    }
}