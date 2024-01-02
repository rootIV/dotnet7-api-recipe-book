using Bogus;
using MyRecipeBook.Communication.Enum;
using MyRecipeBook.Communication.Request;

namespace Unity.Test.Utils.Requests;

public class RequestRegistryRecipeBuilder
{
    public static RequestRecipeJson Build()
    {
        return new Faker<RequestRecipeJson>()
            .RuleFor(r => r.Title, f => f.Commerce.Department())
            .RuleFor(r => r.Category, f => f.PickRandom<Category>())
            .RuleFor(r => r.PreparationMethod, f => f.Lorem.Paragraph())
            .RuleFor(r => r.PreparationTime, f => f.Random.Int(1, 1000))
            .RuleFor(r => r.Ingredients, f => RequestRegistryIngredientBuilder.BuildIngredientList());
    }
}
