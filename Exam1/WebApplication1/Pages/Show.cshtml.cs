using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Pages
{
    public class Show : PageModel
    {
        public Recipe? Recipe { get; set; }

        public Dictionary<Ingredient, double> Ingredients { get; set; } = new Dictionary<Ingredient, double>();

        public string mins = "min";

        public void OnGet(int id, int servings, bool recal)
        {
            if (id != null)
            {
                var db = new DatabaseContext();
                Recipe = db.Recipes.FirstOrDefault(r => r.RecipeId == id)!;
                Functions.GetIngredients(Recipe!);
                foreach (var ingredient in Recipe.Ingredients)
                {
                    var res = db.Ingredients.Find(ingredient.IngredientId)!;
                    Ingredients.Add(res, ingredient.Amount);
                }

                if (recal)
                {
                    var oldRecipe = Recipe;
                    Recipe = new Recipe();
                    Recipe.RecipeId = oldRecipe.RecipeId;
                    Recipe.Category = oldRecipe.Category;
                    Recipe.Name = oldRecipe.Name;
                    Recipe.Servings = servings;
                    Recipe.RecipeText = oldRecipe.RecipeText;
                    Recipe.Time = oldRecipe.Time;
                    if (servings == 0)
                    {
                        servings = Recipe.Servings;
                    }
                    foreach (var ingredient in Ingredients)
                    {
                        Ingredients[ingredient.Key] = ingredient.Value / oldRecipe.Servings * servings;
                    }
                }

                mins = Recipe!.Time % 60 > 0 ? Recipe!.Time % 60+"min" : "";
                Console.WriteLine(Recipe!.Time % 60);
            }
            
        }
    }
}