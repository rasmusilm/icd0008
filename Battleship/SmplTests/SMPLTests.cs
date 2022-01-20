using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Net.Mime;
using BattleshipBasicTypes;
using BattleshipBrain;
using BattleShipConsoleApp;
using DAL;
using Microsoft.EntityFrameworkCore.Design;


namespace SmplTests
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string _basePath;
            Console.WriteLine("Battleship");
            _basePath = args.Length == 1 ? args[0] : Directory.GetCurrentDirectory();

            BSBrain brain = new (10, 10);
            var conf = new GameConfig();
            var confStr = JsonSerializer.Serialize(conf);

            var FileNameStandardConf = "standard.json";
            Console.WriteLine(_basePath);
            File.WriteAllText(Path.Join(_basePath, FileNameStandardConf), confStr);

            object? read = null;
            if (File.Exists(Path.Join(_basePath, FileNameStandardConf)))
            {
                var readString = File.ReadAllText(Path.Join(_basePath, FileNameStandardConf));
                var serializer = new JsonSerializerOptions();
                read = JsonSerializer.Deserialize(readString, typeof(GameConfig), serializer);
            }

            var newread = (GameConfig) read!;
            Console.WriteLine(newread);*/
            /*
            string _basePath;
            Console.WriteLine("Battleship");
            _basePath = args.Length == 1 ? args[0] : Directory.GetCurrentDirectory();
            
            var FileNameStandardConf = "standard.json";
            
            object? read = null;
            if (File.Exists(Path.Join(_basePath, FileNameStandardConf)))
            {
                var readString = File.ReadAllText(Path.Join(_basePath, FileNameStandardConf));
                var serializer = new JsonSerializerOptions();
                read = JsonSerializer.Deserialize(readString, typeof(GameConfig), serializer);
            }

            var newread = (GameConfig) read!;
            Console.WriteLine(newread);
            */
            /*
            int selectedClass = ConsoleNavigator.MultipleChoice(true, "Warrior", "Bard", "Mage", "Archer", 
                "Thief", "Assassin", "Cleric", "Paladin", "etc.");
            
            */
            /*BoardSquareState[][] test = new BoardSquareState[10][];
            test[0] = new BoardSquareState[10];
            test[1] = new BoardSquareState[10];
            
            Console.WriteLine("test" + JsonSerializer.Serialize(test));*/
            /*
            BSBrain brain = new(10, 10);
            /*
            var testBraintext = brain.GetBrainJSON();
            Console.WriteLine(testBraintext);

            Player p1 = new Player(new BoardSquareState[10, 10], new BoardSquareState[10, 10]);

            brain.emptyBoard(p1.getBoard(0));
            brain.emptyBoard(p1.getBoard(1));
            Console.WriteLine(p1.serialize());

            JsonSerializerOptions opt = new JsonSerializerOptions()
            {
                WriteIndented = true,
                AllowTrailingCommas = true,
            };
            
            var _basePath = args.Length == 1 ? args[0] : Directory.GetCurrentDirectory();

            var filepath = _basePath;

            Console.WriteLine(JsonSerializer.Serialize(new BoardSquareState(false, true)));
            Console.WriteLine(JsonSerializer.Serialize(new test()));
            File.WriteAllText(Path.Join(filepath, "brain.json"), testBraintext);
            
            object? read = null;
            object? read2 = null;
            
            if (File.Exists(Path.Join(filepath, "brain.json")))
            {
                var readString = File.ReadAllText(Path.Join(filepath, "brain.json"));
                var info = readString.Split("\nB: ");
                read = JsonSerializer.Deserialize(info[0], typeof(PlayerDTO), opt);
                read2 = JsonSerializer.Deserialize(info[1], typeof(PlayerDTO), opt);
            }

            var pl1 = (PlayerDTO) read!;
            var pl2 = (PlayerDTO) read2!;
            Console.WriteLine(pl1.MyBoard);
            Console.WriteLine(pl2);
            
            BSBrain newBrain = BSBrain.RegnenerateBrain(Player.deserialize(pl1), Player.deserialize(pl2));
            */
            /*
            var brain = new BSBrain(10, 10);
            
            var dto = brain.GetBrainDto();
            var ser = JsonSerializer.Serialize(dto);
            
            var _basePath = args.Length == 1 ? args[0] : Directory.GetCurrentDirectory();
            
            DataHandler.SaveGameToJson(brain, _basePath,"brain.json");

            var newbr = DataHandler.ReadSaveGame(_basePath, "brain.json");

            Console.WriteLine(newbr);

            var regen = JsonSerializer.Deserialize(ser, typeof(BrainDTO));
            var newbrain = (BrainDTO) regen!;

            Console.WriteLine(newbrain.player1.Ships);
            */
            var list = new List<Coordinate>();
            list.Add(new Coordinate(2, 2));
            list.Add(new Coordinate(2, 3));
            Console.WriteLine(list.Contains(new Coordinate(2, 2)));
        }
    }
}