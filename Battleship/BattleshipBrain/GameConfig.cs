using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using BattleshipBasicTypes;
using Domain;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BattleshipBrain
{
    public class GameConfig
    {
        public int BoardWidth { get; set; } = 10;
        public int BoardHeight { get; set; } = 10;
        public EShipPlacement ShipPlacement { get; set; } = EShipPlacement.NoTouch;

        public string Name { get; set; } = "Standard";

        public int Id { get; set; } = 0;
        public List<ShipConfig> ShipConfigs { get; set; } = new()
        {
            new ShipConfig()
            {
                SizeX = 1,
                SizeY = 1,
                Quantity = 4,
                Name = "Destroyer"
            },
            new ShipConfig()
            {
                SizeX = 1,
                SizeY = 2,
                Quantity = 3,
                Name = "Submarine"
            },
            new ShipConfig()
            {
            SizeX = 1,
            SizeY = 3,
            Quantity = 2,
            Name = "Cruiser"
            },
            new ShipConfig()
            {
                SizeX = 1,
                SizeY = 4,
                Quantity = 1,
                Name = "Battleship"
            },
            new ShipConfig()
            {
                SizeX = 1,
                SizeY = 5,
                Quantity = 1,
                Name = "Carrier"
            }
        };

        public int Fields()
        {
            return 5;
        }

        public string[] FieldNames()
        {
            string[] fieldnames = new[]
            {
                "Name",
                "Board height",
                "Board width",
                "Ship placement",
                "Ship types"
            };

            return fieldnames;
        }

        public static GameConfig RegenerateConfig(GameConfigDbDTO gameDto)
        {
            var game = new GameConfig();
            if (gameDto.Name != null) game.Name = gameDto.Name;
            game.BoardHeight = gameDto.BoardHeight;
            game.BoardWidth = gameDto.BoardWidth;
            switch (gameDto.ShipPlacement)
            {
                case "NoTouch":
                    game.ShipPlacement = EShipPlacement.NoTouch;
                    break;
                case "CornerTouch":
                    game.ShipPlacement = EShipPlacement.CornerTouch;
                    break;
                case "SideTouch":
                    game.ShipPlacement = EShipPlacement.SideTouch;
                    break;
            }

            var shipConfigs = JsonSerializer.Deserialize(gameDto.ShipConfigs,
                typeof(List<ShipConfig>), new JsonSerializerOptions());

            if (shipConfigs != null)
            {
                game.ShipConfigs = (List<ShipConfig>) shipConfigs;
            }

            game.Id = gameDto.GameConfigDbDtoId;

            return game;
        }


        public GameConfigDbDTO ToDto()
        {
            var game = new GameConfigDbDTO();
            game.Name = Name;
            game.BoardHeight = BoardHeight;
            game.BoardWidth = BoardWidth;

            switch (ShipPlacement)
            {
                case EShipPlacement.NoTouch:
                    game.ShipPlacement = "NoTouch";
                    break;
                case EShipPlacement.CornerTouch:
                    game.ShipPlacement = "CornerTouch";
                    break;
                case EShipPlacement.SideTouch:
                    game.ShipPlacement = "SideTouch";
                    break;
            }

            game.ShipConfigs = JsonSerializer.Serialize(ShipConfigs);
            return game;
        }

        public override string ToString()
        {
            var repr = new StringBuilder();
            repr.Append("Name: " + Name + "\n");
            repr.Append("Size: " + BoardWidth + " x " + BoardHeight + "\n");
            repr.Append("Ship placement: ");
            switch (ShipPlacement)
            {
                case EShipPlacement.SideTouch:
                    repr.Append("sides can touch");
                    break;
                case EShipPlacement.CornerTouch:
                    repr.Append("can touch diagonally");
                    break;
                case EShipPlacement.NoTouch:
                    repr.Append("ships cant touch");
                    break;
            }
            repr.Append("\n" + "Shiptypes:\n");

            foreach (var config in ShipConfigs)
            {
                repr.Append(config.Name + "\n");
            }
            
            return repr.ToString();
        }
        
        
    }
}