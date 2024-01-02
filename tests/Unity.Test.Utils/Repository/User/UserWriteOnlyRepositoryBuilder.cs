using Moq;
using MyRecipeBook.Domain.Repositorys.User;

namespace Unity.Test.Utils.Repository.User;

public class UserWriteOnlyRepositoryBuilder
{
    private static UserWriteOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserWriteOnlyRepository> _repository;

    private UserWriteOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IUserWriteOnlyRepository>();
    }

    public static UserWriteOnlyRepositoryBuilder Instance()
    {
        _instance = new UserWriteOnlyRepositoryBuilder();
        return _instance;
    }
    public IUserWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}
