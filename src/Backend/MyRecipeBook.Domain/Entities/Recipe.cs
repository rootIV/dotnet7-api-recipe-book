using MyRecipeBook.Domain.Enum;

namespace MyRecipeBook.Domain.Entities;

public class Recipe : BaseEntitie
{
    public string Title { get; set; }
    public Category Category { get; set; }
    public string PreparationMethod { get; set; }
    public int PreparationTime { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
    public long UserId { get; set; }
}
