namespace MyRecipeBook.Domain.Repositorys.Code;

public interface ICodeReadOnlyRepository
{
    Task<Entities.Codes> RecoverEntitieCode(string code);
}
