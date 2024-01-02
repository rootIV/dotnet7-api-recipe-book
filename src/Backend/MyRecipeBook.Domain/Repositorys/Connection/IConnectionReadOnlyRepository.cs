namespace MyRecipeBook.Domain.Repositorys.Connection;

public interface IConnectionReadOnlyRepository
{
    Task<bool> ExistsConnection(long userIdA, long userIdB);
    Task<IList<Entities.User>> RecoverConnectedUsers(long userId);
}
