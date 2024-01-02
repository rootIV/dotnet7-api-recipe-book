namespace MyRecipeBook.Application.Services.UserLogged;

public interface IUserLogged
{
    Task<Domain.Entities.User> RecoverUserLoggedToken();
}
