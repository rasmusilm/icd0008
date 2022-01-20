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
    public class IndexModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public IndexModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CourseDeclaration> CourseDeclaration { get; set; } = default!;

        public async Task OnGetAsync()
        {
            CourseDeclaration = await _context.CourseDeclarations
                .Include(c => c.Course)
                .Include(c => c.Person).ToListAsync();
        }
    }
}
