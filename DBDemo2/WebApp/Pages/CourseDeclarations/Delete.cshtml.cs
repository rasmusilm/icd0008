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
    public class DeleteModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DeleteModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CourseDeclaration = await _context.CourseDeclarations!.FindAsync(id);

            if (CourseDeclaration != null)
            {
                _context.CourseDeclarations.Remove(CourseDeclaration);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
