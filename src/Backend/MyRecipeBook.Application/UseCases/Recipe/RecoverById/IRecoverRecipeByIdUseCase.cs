using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Recipe.RecoverById;

public interface IRecoverRecipeByIdUseCase
{
    Task<ResponseRecipeJson> Execute(long recipeId);
}
