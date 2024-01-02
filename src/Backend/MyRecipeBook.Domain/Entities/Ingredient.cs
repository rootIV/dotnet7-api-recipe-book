using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities;

[Table("Ingredients")]
public class Ingredient : BaseEntitie
{
    public long RecipeId { get; set; }
    public string Product { get; set; }
    public string Quantity { get; set; }
}
