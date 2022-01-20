using System.Collections.Generic;

namespace Domain
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        
        public string? Name { get; set; }
        
        public string? Category { get; set; }
        
        public int Servings { get; set; }
        
        public int Time { get; set; }
        
        public string? RecipeText { get; set; }

        public List<IngredientInRecipe> Ingredients { get; set; } = default!;
    }
}