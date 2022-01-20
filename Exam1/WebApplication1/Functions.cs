using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;

namespace WebApplication1
{
    public class Functions
    {
        public static Recipe GetIngredients(Recipe recipe)
        {
            var db = new DatabaseContext();
            recipe.Ingredients = db.IngredientsInRecipe.Where(inRecipe => inRecipe.RecipeId == recipe.RecipeId).ToList();
            db.SaveChanges();
            return recipe;
        }

        public static bool SearchAvailability(Recipe recipe, List<Ingredient> ingredients)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                foreach (var ingredient2 in ingredients)
                {
                    if (ingredient2.IngredientId == ingredient.IngredientId)
                    {
                        if (ingredient2.Amount < ingredient.Amount)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        
        public static bool InNumberConstraints(Recipe recipe, int minTime, int maxTime, int minServe, int maxServe)
        {
            if (recipe.Time > maxTime || recipe.Time < minTime)
            {
                return false;
            }

            if (recipe.Servings > maxServe || recipe.Servings < minServe)
            {
                return false;
            }

            return true;
        }

        public static bool CheckDesiredIngredients(Recipe recipe, List<Ingredient> ingredients, string[] names)
        {
            foreach (var name in names)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    var ingredientName = ingredients.Find(i => i.IngredientId == ingredient.IngredientId)!.Name;
                    if (ingredientName == name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        public static bool CheckUnDesiredIngredients(Recipe recipe, List<Ingredient> ingredients, string[] names)
        {
            foreach (var name in names)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    var ingredientName = ingredients.Find(i => i.IngredientId == ingredient.IngredientId)!.Name;
                    if (ingredientName == name)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool NameContainsKeys(Recipe recipe, string[] keys)
        {
            foreach (var comb in keys)
            {
                if (recipe.Name!.Contains(comb))
                {
                    return true;
                }
            }
            return false;
        }
        
        /*
         if (recipe.Time > maxTime || recipe.Time < minTime)
                    {
                        cantInclude = true;
                    }

                    if (recipe.Servings > maxServe || recipe.Servings < minServe)
                    {
                        cantInclude = true;
                    }

                    foreach (var text in toAvoid)
                    {
                        if (recipe.Name!.Contains(text))
                        {
                            cantInclude = true;
                        }
                    }

                    foreach (var text in toFind)
                    {
                        if (!recipe.Name!.Contains(text))
                        {
                            cantInclude = true;
                        }
                    }
                    
                    var inOne = true;

                    foreach (var ingredientName in include)
                    {
                        inOne = false;
                        foreach (var ingredient in recipe.Ingredients)
                        {
                            var res = ingredients.Find(ingr => ingr.IngredientId == ingredient.IngredientId)!;
                            if (res.Name!.Contains(ingredientName))
                            {
                                inOne = true;
                                break;
                            }
                        }

                        if (inOne)
                        {
                            break;
                        }
                    }

                    var notInOne = false;
                    
                    foreach (var ingredientName in includeNot)
                    {
                        notInOne = true;
                        foreach (var ingredient in recipe.Ingredients)
                        {
                            var res = ingredients.Find(ingr => ingr.IngredientId == ingredient.IngredientId)!;
                            if (res.Name!.Contains(ingredientName))
                            {
                                notInOne = false;
                                break;
                            }
                        }

                        if (!notInOne)
                        {
                            break;
                        }
                    }
         */
    }
}