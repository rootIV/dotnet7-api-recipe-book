using MyRecipeBook.Communication.Enum;

namespace MyRecipeBook.Communication.Request;

public class RequestDashboardJson
{
    public string TitleOrIngredient { get; set; }
    public Category? Category { get; set; }
}
