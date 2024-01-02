namespace MyRecipeBook.Domain.Repositorys.Code;

public interface ICodeWriteOnlyRepository
{
    Task Registry(Entities.Codes code);
    Task Delete(long userId);
}
