namespace MyRecipeBook.Domain.Repositorys;

public interface IUnityOfWork
{
    Task Commit();
}
