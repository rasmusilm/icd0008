using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages
{
    public class editconf : PageModel
    {
        private readonly BSDBContext _ctx;

        public editconf(BSDBContext ctx)
        {
            _ctx = ctx;
        }
    
        [BindProperty]
        public GameConfigDbDTO? ConfigDb { get; set; }
    
        public void OnGet(int? id)
        {
            if (id is null or 0)
            {
                return;
            }
            ConfigDb = _ctx.Configurations?.Find(id);
        }

        public RedirectResult OnPost()
        {
            _ctx.Configurations!.Update(ConfigDb!);
            _ctx.SaveChanges();

            return new RedirectResult("/Game/configurations");
        }
    }
}