using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class ChangeIngrdients : PageModel
    {
        public List<Ingredient> Ingredients { get; set; } = new ();
        public Dictionary<Ingredient, double> RecipeIngredients { get; set; } = new ();
        
        public int Id { get; set; }
        public void OnGet(int id)
        {
            var db = new DatabaseContext();
            Ingredients = db.Ingredients.Where(ingredient => true).ToList();
            var recipe = Functions.GetIngredients(db.Recipes.Find(id));
            foreach (var ingredient in recipe.Ingredients)
            {
                var res = db.Ingredients.Find(ingredient.IngredientId)!;
                RecipeIngredients.Add(res, ingredient.Amount);
            }

            Id = id;
        }

        public void OnPost()
        {
            var db = new DatabaseContext();
            Ingredients = db.Ingredients.Where(ingredient => true).ToList();
            var requestKeys = Request.Form.Keys.ToList();
            requestKeys.ForEach(Console.WriteLine);
            foreach (var key in requestKeys)
            {
                if (key.Contains("amount"))
                {
                    var keyId = key.Split("t")[1];
                    Console.WriteLine(Request.Form[key]);
                    var ingredient = db.Ingredients.Find(int.Parse(keyId));
                    if (RecipeIngredients.ContainsKey(ingredient))
                    {
                        RecipeIngredients[ingredient] = double.Parse(Request.Form[key]);
                    }
                    
                }
                else if (key != "id" & key != "__RequestVerificationToken")
                {
                    Console.WriteLine(key);
                    RecipeIngredients.Add(db.Ingredients.Find(int.Parse(key)), 0);
                }
            }

            var RecipeIngredientsDb = new Dictionary<Ingredient, double>();
            var recipe = db.Recipes.Find(int.Parse(Request.Form["id"]));
            
            db.IngredientsInRecipe.RemoveRange(db.IngredientsInRecipe.Where(n => n.RecipeId == recipe.RecipeId));

            foreach (var ingredient in RecipeIngredients)
            {
                db.IngredientsInRecipe.Add(new IngredientInRecipe()
                {
                    IngredientId = ingredient.Key.IngredientId, RecipeId = recipe.RecipeId, Amount = ingredient.Value
                });
            }

            db.SaveChanges();
        }
    }
}