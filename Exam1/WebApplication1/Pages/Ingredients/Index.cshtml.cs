using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApplication1.Pages_Ingredients
{
    public class IndexModel : PageModel
    {
        private readonly DAL.DatabaseContext _context;

        public IndexModel(DAL.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Ingredient> Ingredient { get;set; }  = default!;

        public async Task OnGetAsync()
        {
            Ingredient = await _context.Ingredients.ToListAsync();
        }
    }
}
