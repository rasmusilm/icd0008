namespace Domain
{
    public class BrainDTO
    {
        public PlayerDTO player1 { get; set; } = null!;
        public PlayerDTO player2 { get; set; } = null!;

        public string Name { get; set; } = "";

        public string Config { get; set; } = "";

        public string History { get; set; } = "";

        public BrainDTO()
        {
            
        }

        public BrainDTO(PlayerDTO pl1, PlayerDTO pl2, string config)
        {
            player1 = pl1;
            player2 = pl2;
            Config = config;
        }
    }
}