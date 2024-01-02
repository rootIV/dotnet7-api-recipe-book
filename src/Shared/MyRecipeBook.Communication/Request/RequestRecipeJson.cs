using MyRecipeBook.Communication.Enum;

namespace MyRecipeBook.Communication.Request;

public class RequestRecipeJson
{
    public string Title { get; set; }
    public Category Category { get; set; }
    public string PreparationMethod { get; set; }
    public int PreparationTime { get; set; }
    public List<RequestIngredientJson> Ingredients { get; set; }

    public RequestRecipeJson()
    {
        Ingredients = new();
    }
}
