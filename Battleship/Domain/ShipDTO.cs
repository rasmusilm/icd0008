using System.Collections.Generic;
using BattleshipBasicTypes;

namespace Domain
{
    public class ShipDTO
    {
        public string name { get; set; } = "";
        public List<Coordinate> coordinates { get; set; } = new();
        
        public ShipDTO(){}

        public ShipDTO(string shipName, List<Coordinate> shipCoordinates)
        {
            name = shipName;
            coordinates = shipCoordinates;
        }

        public static ShipDTO toDto(Ship ship)
        {
            return new ShipDTO(ship.GetName(), ship.GetCoordinates());
        }

        public static List<ShipDTO> ToDtoList(List<Ship> ships)
        {
            var dtoList = new List<ShipDTO>();
            foreach (var ship in ships)
            {
                dtoList.Add(toDto(ship));
            }

            return dtoList;
        }
    }
    
    
}