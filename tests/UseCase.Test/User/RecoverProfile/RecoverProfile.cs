using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.RecoverProfile;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Mapper;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.User.RecoverProfile;

public class RecoverProfile
{
    [Fact]
    public async Task Validate_Success()
    {
        (var _user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(_user);

        var response = await useCase.Execute();

        response.Should().NotBeNull();
        response.Name.Should().Be(_user.Name);
        response.Email.Should().Be(_user.Email);
        response.Phone.Should().Be(_user.Phone);
    }

    private static RecoverProfileUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Instance();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();

        return new RecoverProfileUseCase(mapper, userLogged);
    }
}
