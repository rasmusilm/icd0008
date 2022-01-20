using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BattleshipBasicTypes;
using Domain;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BattleshipBrain
{
    public class MoveHistory
    {
        public List<Move> MovesDone = new List<Move>();
        public List<Move> MovesUndone = new List<Move>();
        public int ID = 0;
        public int gameId = 0;

        public MoveHistoryDTO GetDto()
        {
            var dto = new MoveHistoryDTO();
            dto.Redo = JsonSerializer.Serialize(MovesUndone, new JsonSerializerOptions());
            dto.Undo = JsonSerializer.Serialize(MovesDone, new JsonSerializerOptions());
            if (ID != 0)
            {
                dto.MoveHistoryDTOId = ID;
            }
            if (gameId != 0)
            {
                dto.BrainDbDTOId = gameId;
            }
            return dto;
        }

        public static MoveHistory getFromDto(MoveHistoryDTO dto)
        {
            var history = new MoveHistory();
            history.MovesUndone = JsonSerializer.Deserialize<List<Move>>(dto.Redo, new JsonSerializerOptions())!;
            history.MovesDone = JsonSerializer.Deserialize<List<Move>>(dto.Undo, new JsonSerializerOptions())!;
            history.ID = dto.MoveHistoryDTOId;
            return history;
        }

        public Move Undo()
        {
            if (MovesDone.Count == 0)
            {
                throw new RuntimeBinderException("No moves to undone");
            }
            var move = MovesDone.Last();
            MovesUndone.Add(move);
            MovesDone.Remove(move);
            return move;
        }

        public Move Redo()
        {
            var move = MovesUndone.Last();
            MovesUndone.Remove(move);
            return move;
        }

        public void Do(string action, int playerDes, int x, int y)
        {
            MovesDone.Add(new Move(playerDes, action, x, y));
        }

        public int GetUndoStackSize()
        {
            return MovesDone.Count;
        }

        public int GetRedoStackSize()
        {
            return MovesUndone.Count;
        }
    }
    
    
}