using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public interface ILoginUserUseCase
{
    Task<Communication.Response.ResponseLoginJson> Execute(Communication.Request.RequestLoginJson request);
}
