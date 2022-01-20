using System.Collections.Generic;
using System.Text.Json;
using BattleshipBasicTypes;

namespace Domain
{
    public class MoveHistoryDTO
    {
        public int MoveHistoryDTOId { get; set; }
        public string Undo { get; set; } = JsonSerializer.Serialize(new List<Move>(), new JsonSerializerOptions());
        public string Redo { get; set; } = JsonSerializer.Serialize(new List<Move>(), new JsonSerializerOptions());
        public int BrainDbDTOId { get; set; }
    }
}