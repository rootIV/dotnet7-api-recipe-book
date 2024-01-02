using Moq;
using MyRecipeBook.Application.Services.UserLogged;

namespace Unity.Test.Utils.UserLogged;

public class UserLoggedBuilder
{
    private static UserLoggedBuilder _instance;
    private readonly Mock<IUserLogged> _repository;

    private UserLoggedBuilder()
    {
        _repository ??= new Mock<IUserLogged>();
    }

    public static UserLoggedBuilder Instance()
    {
        _instance = new UserLoggedBuilder();
        return _instance;
    }
    public UserLoggedBuilder RecoverUser(MyRecipeBook.Domain.Entities.User user)
    {
        _repository.Setup(c => c.RecoverUserLoggedToken()).ReturnsAsync(user);

        return this;
    }
    public IUserLogged Build()
    {
        return _repository.Object;
    }
}
