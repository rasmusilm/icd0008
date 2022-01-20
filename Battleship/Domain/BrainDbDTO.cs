namespace Domain
{
    public class BrainDbDTO
    {
        public int BrainDbDTOId { get; set; }
        
        public string? GameName { get; set; }
        
        public int Turn { get; set; }
        
        public int GameConfigDbDtoId { get; set; }
    }
}