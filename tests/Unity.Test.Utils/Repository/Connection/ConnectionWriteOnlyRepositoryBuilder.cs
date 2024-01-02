using Moq;
using MyRecipeBook.Domain.Repositorys.Connection;

namespace Unity.Test.Utils.Repository.Connection;

public class ConnectionWriteOnlyRepositoryBuilder
{
    private static ConnectionWriteOnlyRepositoryBuilder _instance;
    private readonly Mock<IConnectionWriteOnlyRepository> _repository;

    private ConnectionWriteOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IConnectionWriteOnlyRepository>();
    }

    public static ConnectionWriteOnlyRepositoryBuilder Instance()
    {
        _instance = new ConnectionWriteOnlyRepositoryBuilder();
        return _instance;
    }
    public IConnectionWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}
