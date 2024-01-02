namespace MyRecipeBook.Application.UseCases.Connection.Remove;

public interface IRemoveConnectionsUseCase
{
    Task Execute(long connectedUserIdToRemove);
}
