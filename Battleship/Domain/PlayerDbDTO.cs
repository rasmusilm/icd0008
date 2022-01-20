namespace Domain
{
    public class PlayerDbDTO
    {
        public int PlayerDbDTOId { get; set; }

        public string? MyBoard { get; set; }
        
        public string? EnemyBoard { get; set; }

        public string? Ships { get; set; }

        public int BrainDbDTOId { get; set; }
    }
}