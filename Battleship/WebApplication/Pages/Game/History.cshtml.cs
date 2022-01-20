using System;
using BattleshipBasicTypes;
using BattleshipBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class History : PageModel
    {
        
        public static BSBrain Game = new BSBrain(10, 10);

        public BoardSquareState[,] Own = Game.getBoard(0, 0);

        public BoardSquareState[,] Enemy = Game.getBoard(0, 1);

        public string Id = "0";
        
        public int UndoSize = 0;
        public int RedoSize = 0;
        
        public bool GameOver = false;

        public int Winner = -1;
        
        public void OnGet(string rewind, string id)
        {
            if (Functions.useDB())
            {
                Game = DataHandler.ReadSaveGameDB(int.Parse(id));
                Own = Game.getBoard(0, 0);
                Enemy = Game.getBoard(0, 1);
                Id = id;
                
                if (!string.IsNullOrEmpty(rewind))
                {
                    switch (rewind)
                    {
                        case "undo":
                            Game.Undo();
                            Own = Game.getBoard(0, 0);
                            Enemy = Game.getBoard(0, 1);
                            break;
                        case "redo":
                            Game.Redo();
                            Own = Game.getBoard(0, 0);
                            Enemy = Game.getBoard(0, 1);
                            break;
                        case "continue":
                            Game.Continue();
                            Own = Game.getBoard(0, 0);
                            Enemy = Game.getBoard(0, 1);
                            break;
                        case "begin":
                            Game.ToBeginning();
                            Own = Game.getBoard(0, 0);
                            Enemy = Game.getBoard(0, 1);
                            break;
                    }
                }
            
                DataHandler.UpdateSaveGameDB(Game, int.Parse(id));
            }
            else
            {
                Game = DataHandler.ReadSaveGame(Functions.GetGameJsonPath(), id);
                Own = Game.getBoard(0, 0);
                Enemy = Game.getBoard(0, 1);
                Id = id;
                
                if (!string.IsNullOrEmpty(rewind))
                {
                    switch (rewind)
                    {
                        case "undo":
                            Game.Undo();
                            Own = Game.getBoard(0, 0);
                            Enemy = Game.getBoard(0, 1);
                            break;
                        case "redo":
                            Game.Redo();
                            Own = Game.getBoard(0, 0);
                            Enemy = Game.getBoard(0, 1);
                            break;
                        case "continue":
                            Game.Continue();
                            Own = Game.getBoard(0, 0);
                            Enemy = Game.getBoard(0, 1);
                            break;
                        case "begin":
                            Game.ToBeginning();
                            Own = Game.getBoard(0, 0);
                            Enemy = Game.getBoard(0, 1);
                            break;
                    }
                }
                DataHandler.SaveGameToJsonOverwrite(Game,Functions.GetGameJsonPath(), id);
            }
            
            UndoSize = Game.GetHistory().GetUndoStackSize();
            RedoSize = Game.GetHistory().GetRedoStackSize();
            
            var winner = Game.FindWinner();
            if (winner != -1)
            {
                GameOver = true;
                Winner = winner;
            }
        }
    }
}