using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.User.Registry;

public interface IRegistryUserUseCase
{
    Task<Communication.Response.ResponseRegistryUserJson> Execute(Communication.Request.RequestRegistryUserJson request);
}
