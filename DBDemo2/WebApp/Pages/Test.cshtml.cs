using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Test : PageModel
    {
        private readonly ApplicationDbContext _ctx;
        public string? Name { get; set; }

        public Test(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<Person> Persons { get; set; } = default!;
        
        public void OnGet(string name)
        {
            Name = name;
            
            Persons = _ctx.Persons.ToList();
        }
    }
}