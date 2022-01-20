using BattleshipBrain;
using DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class Settings : PageModel
    {
        public bool savetToDB = Functions.useDB();
        
        public void OnGet(int switchsave)
        {
            if (switchsave == 1)
            {
                Functions.switchSaving();
                savetToDB = Functions.useDB();
            }
        }
    }
}