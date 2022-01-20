using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IngredientsInRecipe_IngredientId",
                table: "IngredientsInRecipe",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsInRecipe_Ingredients_IngredientId",
                table: "IngredientsInRecipe",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsInRecipe_Ingredients_IngredientId",
                table: "IngredientsInRecipe");

            migrationBuilder.DropIndex(
                name: "IX_IngredientsInRecipe_IngredientId",
                table: "IngredientsInRecipe");
        }
    }
}
