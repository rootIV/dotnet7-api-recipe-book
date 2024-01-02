using Moq;
using MyRecipeBook.Domain.Repositorys.User;

namespace Unity.Test.Utils.Repository.User;

public class UserUpdateOnlyRepositoryBuilder
{
    private static UserUpdateOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserUpdateOnlyRepository> _repository;

    private UserUpdateOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IUserUpdateOnlyRepository>();
    }

    public static UserUpdateOnlyRepositoryBuilder Instance()
    {
        _instance = new UserUpdateOnlyRepositoryBuilder();
        return _instance;
    }
    public UserUpdateOnlyRepositoryBuilder RecoverById(MyRecipeBook.Domain.Entities.User user)
    {
        _repository.Setup(c => c.RecoverUserById(user.Id)).ReturnsAsync(user);

        return this;
    }
    public IUserUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }
}
