using System;
using System.Collections.Generic;

namespace Domain
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        
        public string? Name { get; set; }
        
        public string? Category { get; set; }
        
        public string? Location { get; set; }
        
        public double Amount { get; set; }
        
        public string? Unit { get; set; }
        
        public List<IngredientInRecipe> RecipesContaining { get; set; } = default!;
    }
}