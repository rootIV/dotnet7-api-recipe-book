namespace MyRecipeBook.Application.UseCases.Connection.AcceptConnection;

public interface IAcceptConnectionUseCase
{
    Task<string> Execute(string userToConnectId);
}
