using MyRecipeBook.Communication.Enum;

namespace MyRecipeBook.Communication.Response;

public class ResponseRecipeJson
{
    public string Id { get; set; }
    public string Title { get; set; }
    public Category Category { get; set; }
    public string PreparationMethod { get; set; }
    public int PreparationTime { get; set; }
    public List<ResponseIngredientJson> Ingredients { get; set; }
}
