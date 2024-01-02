namespace MyRecipeBook.Domain.Repositorys.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistUserEmail(string email);
    Task<Entities.User> RecoverUserByEmailPass(string email, string password);
    Task<Entities.User> RecoverUserByEmail(string email);
}
