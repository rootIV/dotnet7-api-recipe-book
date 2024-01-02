namespace MyRecipeBook.Domain.Repositorys.Connection;

public interface IConnectionWriteOnlyRepository
{
    Task Registry(Entities.Connection connection);
    Task RemoveConnection(long userId, long connectedUserIdToRemove);
}
