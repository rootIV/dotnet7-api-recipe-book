using Moq;
using MyRecipeBook.Domain.Repositorys.Connection;

namespace Unity.Test.Utils.Repository.Connection;

public class ConnectionReadOnlyRepositoryBuilder
{
    private static ConnectionReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IConnectionReadOnlyRepository> _repository;

    private ConnectionReadOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IConnectionReadOnlyRepository>();
    }

    public static ConnectionReadOnlyRepositoryBuilder Instance()
    {
        _instance = new ConnectionReadOnlyRepositoryBuilder();
        return _instance;
    }
    public ConnectionReadOnlyRepositoryBuilder ExistsConnection(long? userIdA, long? userIdB)
    {
        if (userIdA.HasValue && userIdB.HasValue)
        {
            _repository.Setup(x => x.ExistsConnection(userIdA.Value, userIdB.Value)).ReturnsAsync(true);
        }

        return this;
    }
    public ConnectionReadOnlyRepositoryBuilder RecoverConnections(MyRecipeBook.Domain.Entities.User user, 
        IList<MyRecipeBook.Domain.Entities.User> connections)
    {
        _repository.Setup(x => x.RecoverConnectedUsers(user.Id)).ReturnsAsync(connections);

        return this;
    }
    public IConnectionReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
