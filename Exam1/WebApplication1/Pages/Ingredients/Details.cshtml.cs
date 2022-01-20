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
    public class DetailsModel : PageModel
    {
        private readonly DAL.DatabaseContext _context;

        public DetailsModel(DAL.DatabaseContext context)
        {
            _context = context;
        }

        public Ingredient Ingredient { get; set; }  = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ingredient = await _context.Ingredients.FirstOrDefaultAsync(m => m.IngredientId == id);

            if (Ingredient == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
