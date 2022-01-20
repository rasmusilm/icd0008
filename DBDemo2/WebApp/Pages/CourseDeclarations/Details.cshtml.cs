using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_CourseDeclarations
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DetailsModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public CourseDeclaration? CourseDeclaration { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CourseDeclaration = await _context.CourseDeclarations
                .Include(c => c.Course)
                .Include(c => c.Person).FirstOrDefaultAsync(m => m.CourseDeclarationId == id);

            if (CourseDeclaration == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
