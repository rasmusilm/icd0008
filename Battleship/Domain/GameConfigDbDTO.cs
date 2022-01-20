namespace Domain
{
    public class GameConfigDbDTO
    {
        public int GameConfigDbDtoId { get; set; }
        
        public int BoardWidth { get; set; }
        
        public int BoardHeight { get; set; }
        
        public string? ShipPlacement { get; set; }
        
        public string? ShipConfigs { get; set; }
        
        public string? Name { get; set; }
    }
}