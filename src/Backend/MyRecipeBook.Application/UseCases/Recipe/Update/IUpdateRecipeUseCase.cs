using MyRecipeBook.Communication.Request;

namespace MyRecipeBook.Application.UseCases.Recipe.Update;

public interface IUpdateRecipeUseCase
{
    Task Execute(long recipeId, RequestRecipeJson request);
}
