using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositorys.Code;

namespace Unity.Test.Utils.Repository.Code;

public class CodeReadOnlyRepositoryBuilder
{
    private static CodeReadOnlyRepositoryBuilder _instance;
    private readonly Mock<ICodeReadOnlyRepository> _repository;

    private CodeReadOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<ICodeReadOnlyRepository>();
    }

    public static CodeReadOnlyRepositoryBuilder Instance()
    {
        _instance = new CodeReadOnlyRepositoryBuilder();
        return _instance;
    }
    public CodeReadOnlyRepositoryBuilder RecoverEntitieCode(Codes code)
    {
        _repository.Setup(x => x.RecoverEntitieCode(code.Code)).ReturnsAsync(code);
        return this;
    }
    public ICodeReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
