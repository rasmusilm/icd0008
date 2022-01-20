using System;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class EditPerson : PageModel
    {
        private readonly ApplicationDbContext _ctx;

        public EditPerson(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        
        [BindProperty]
        public Person? Person { get; set; }
        public void OnGet(int? id)
        {
            if (id is null or 0)
            {
                return;
            }
            Person = _ctx.Persons?.Find(id);
        }

        public void OnPost()
        {
            Console.WriteLine(Person);
            _ctx.Persons!.Update(Person!);
            _ctx.SaveChanges();
        }
    }
}