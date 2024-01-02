using Bogus;
using MyRecipeBook.Communication.Request;

namespace Unity.Test.Utils.Requests;

public class RequestRegistryIngredientBuilder
{
    public static List<RequestIngredientJson> BuildIngredientList(int? value = 3)
    {
        var faker = new Faker<RequestIngredientJson>()
            .RuleFor(i => i.Quantity, f => f.Lorem.Sentence())
            .RuleFor(i => i.Product, f => f.Random.Word());

        return faker.Generate(value.Value);
    }
}
