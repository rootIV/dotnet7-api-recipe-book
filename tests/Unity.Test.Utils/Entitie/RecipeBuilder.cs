using Bogus;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enum;

namespace Unity.Test.Utils.Entitie;

public class RecipeBuilder
{
    public static Recipe Build(User user)
    {
        return new Faker<Recipe>()
            .RuleFor(c => c.Id, _ => user.Id)
            .RuleFor(r => r.Title, f => f.Commerce.Department())
            .RuleFor(r => r.Category, f => f.PickRandom<Category>())
            .RuleFor(r => r.PreparationMethod, f => f.Lorem.Paragraph())
            .RuleFor(r => r.PreparationTime, f => f.Random.Int(1, 1000))
            .RuleFor(r => r.Ingredients, f => RandomIngredients(f, user.Id))
            .RuleFor(r => r.UserId, _ => user.Id);
    }
    private static List<Ingredient> RandomIngredients(Faker faker, long userId)
    {
        List<Ingredient> ingredients = new();

        for (int i = 0; i < faker.Random.Int(1, 10); i++)
        {
            ingredients.Add(new Ingredient
            {
                Id = (userId * 100) + (i + 1),
                Product = faker.Commerce.ProductName(),
                Quantity = $"{faker.Random.Double(1, 10)} {faker.Random.Word()}"
            });
        }

        return ingredients;
    }
}
