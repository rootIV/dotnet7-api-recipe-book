namespace MyRecipeBook.Domain.Repositorys.User;

public interface IUserWriteOnlyRepository
{
    Task Add(Entities.User user);
}
