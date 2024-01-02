using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.User.RecoverProfile;

public interface IRecoverProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
