namespace MyRecipeBook.Domain.Repositorys.Recipe;

public interface IRecipeWriteOnlyRepository
{
    Task Registry(Entities.Recipe recipe);
    Task Delete(long recipeId);
}
