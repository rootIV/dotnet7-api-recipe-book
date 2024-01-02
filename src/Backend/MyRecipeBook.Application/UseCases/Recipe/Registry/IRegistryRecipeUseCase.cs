using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Recipe.Registry;

public interface IRegistryRecipeUseCase
{
    Task<Communication.Response.ResponseRecipeJson> Execute(Communication.Request.RequestRecipeJson request);
}
