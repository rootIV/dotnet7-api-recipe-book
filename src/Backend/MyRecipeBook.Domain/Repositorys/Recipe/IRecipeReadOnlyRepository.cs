namespace MyRecipeBook.Domain.Repositorys.Recipe;

public interface IRecipeReadOnlyRepository
{
    Task<IList<Entities.Recipe>> RecoverUserAllRecipes(long userId);
    Task<Entities.Recipe> RecoverRecipeById(long userId);
    Task<int> RecipeQuantity(long userId);
    Task<IList<Entities.Recipe>> RecoverAllRecipesFromConnectedUsers(List<long> userIds);
}
