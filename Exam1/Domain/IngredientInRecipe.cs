namespace Domain
{
    public class IngredientInRecipe
    {
        public int IngredientInRecipeId { get; set; }
        
        public int IngredientId { get; set; }
        
        public int RecipeId { get; set; }
        
        public double Amount { get; set; }
    }
}