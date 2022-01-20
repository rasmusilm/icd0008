using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Grades
{
    public class IndexModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public IndexModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Grade> Grade { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Grade = await _context.Grades
                .Include(g => g.Declaration)
                .Include(g => g.Homework).ToListAsync();
        }
    }
}
