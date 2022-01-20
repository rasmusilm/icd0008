using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Pages.Game
{
    public class saveApi : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]  
        public IActionResult FormsTestPost()  
        {
            Console.Write(HttpContext.Request.Body.Length);
            Console.WriteLine(HttpContext.Request.Body.Length);
            return Content("Hello, " + HttpContext.Request.Form["UserName"] + ". You are " + HttpContext.Request.Form["UserAge"] + " years old!");  
        }
    }
}