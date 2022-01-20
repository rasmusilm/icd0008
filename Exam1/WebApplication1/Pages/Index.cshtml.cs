using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Recipe> Recipes;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            var db = new DatabaseContext();
            Recipes = db.Recipes.Where(x => true).ToList();
        }

        public void OnGet()
        {
            var db = new DatabaseContext();
        }
    }
}