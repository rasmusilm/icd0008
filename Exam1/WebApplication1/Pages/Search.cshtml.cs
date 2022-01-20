using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class Search : PageModel
    {
        public bool searched = false;
        public List<Recipe> Results = new List<Recipe>();
        public void OnGet(string? possible, string search, string? with, string? containing, 
            string? without, string? containt, string? serve_min, string? serve_max, 
            string? time_min, string? time_max)
        {
            var toFind = new string[] {};
            var include = new string[] {};
            var includeNot = new string[] {};
            bool avoidIngredients = false;
            var requireIngredients = false;
            
            bool searchPossibleOnes = possible == "on";
            if (!string.IsNullOrEmpty(search))
            {
                Console.WriteLine(search);
                toFind = search.Split("; ");
                foreach (var s in toFind)
                {
                    Console.WriteLine(s);
                }
                searched = true;
            }

            if (!string.IsNullOrEmpty(with))
            {
                requireIngredients = true;
                include = containing!.Split(";");
                searched = true;
            }
            if (!string.IsNullOrEmpty(without))
            {
                avoidIngredients = true;
                includeNot = containt!.Split(";");
                searched = true;
            }

            int minServe = 0, maxServe = 0, minTime = 0, maxTime = 0;

            if (!string.IsNullOrEmpty(serve_min) || !string.IsNullOrEmpty(time_min))
            {
                searched = true;
                minServe = int.Parse(serve_min);
                maxServe = int.Parse(serve_max);
                minTime = int.Parse(time_min);
                maxTime = int.Parse(time_max);
            }

            if (searched)
            {
                var db = new DatabaseContext();
                var all = db.Recipes.ToList();
                var ingredients = db.Ingredients.ToList();
                var canBeDone = true;
                foreach (var re in all)
                {
                    var recipe = Functions.GetIngredients(re);
                    var inOne = true;
                    var notInOne = true;
                    var nameMatces = true;
                    var canInclude = Functions.InNumberConstraints(recipe, minTime, maxTime, minServe, maxServe);
                    if (searchPossibleOnes)
                    {
                        canBeDone = Functions.SearchAvailability(recipe, ingredients);
                    }

                    if (requireIngredients)
                    {
                        inOne = Functions.CheckDesiredIngredients(recipe, ingredients, include);
                    }

                    if (avoidIngredients)
                    {
                        notInOne = Functions.CheckUnDesiredIngredients(recipe, ingredients, includeNot);
                    }

                    if (toFind.Length != 0)
                    {
                        nameMatces = Functions.NameContainsKeys(recipe, toFind);
                    }

                    Console.WriteLine(canBeDone);
                    Console.WriteLine(canInclude);
                    Console.WriteLine(inOne);
                    Console.WriteLine(notInOne);
                    
                    if (canBeDone && canInclude && inOne && notInOne && nameMatces)
                    {
                        Results.Add(recipe);
                    }
                }
            }

            
        }
    }
}