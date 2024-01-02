using Moq;
using MyRecipeBook.Domain.Repositorys.User;

namespace Unity.Test.Utils.Repository.User;

public class UserReadOnlyRepositoryBuilder
{
    private static UserReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserReadOnlyRepository> _repository;

    private UserReadOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IUserReadOnlyRepository>();
    }

    public static UserReadOnlyRepositoryBuilder Instance()
    {
        _instance = new UserReadOnlyRepositoryBuilder();
        return _instance;
    }
    public UserReadOnlyRepositoryBuilder ExistsUserWithEmail(string email)
    {
        if (!string.IsNullOrEmpty(email))
            _repository.Setup(i => i.ExistUserEmail(email)).ReturnsAsync(true);

        return this;
    }
    public UserReadOnlyRepositoryBuilder RecorverByEmailPass(MyRecipeBook.Domain.Entities.User user)
    {
        _repository.Setup(i => i.RecoverUserByEmailPass(user.Email, user.Password)).ReturnsAsync(user);

        return this;
    }
    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
