using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Connection.Recover;

public interface IRecoverAllConnectionsUseCase
{
    Task<ResponseUserConnectionsJson> Execute();
}
