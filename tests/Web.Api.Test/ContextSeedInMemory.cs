using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Infrastructure.RepositoryAcess;
using Unity.Test.Utils.Entitie;

namespace Web.Api.Test;

public class ContextSeedInMemory
{
    public static (User user, string pass) Seed(MyRecipeBookContext context)
    {
        (var user, string pass) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        context.Users.Add(user);
        context.Recipes.Add(recipe);

        context.SaveChanges();

        return (user, pass);
    }
    public static (User user, string pass) SeedWithouRecipe(MyRecipeBookContext context)
    {
        (var user, string pass) = UserBuilder.Build();

        context.Users.Add(user);

        context.SaveChanges();

        return (user, pass);
    }
    public static (User user, string pass) SeedUserWithConnection(MyRecipeBookContext context)
    {
        (var user, string pass) = UserBuilder.Build();

        context.Users.Add(user);

        var userConnections = ConnectionBuilder.Build();

        for (var index = 1; index <= userConnections.Count; index++)
        {
            var connectionWithUser = userConnections[index - 1];

            context.Connections.Add(new Connection
            {
                Id = index,
                UserId = user.Id,
                ConnectedWithUser = connectionWithUser,
            });
        }

        context.SaveChanges();

        return (user, pass);
    }
}
