using MyRecipeBook.Communication.Request;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
