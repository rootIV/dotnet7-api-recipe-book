namespace MyRecipeBook.Domain.Repositorys.Recipe;

public interface IRecipeUpdateOnlyRepository
{
    Task<Entities.Recipe> RecoverRecipeById(long recipeId);
    void Update(Entities.Recipe recipe);
}
