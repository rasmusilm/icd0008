using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Test : PageModel
    {
        public string? Name { get; set; }
        
        public void OnGet(string name)
        {
            Name = name;
        }
    }
}