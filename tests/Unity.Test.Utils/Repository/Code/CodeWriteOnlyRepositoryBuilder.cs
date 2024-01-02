using Moq;
using MyRecipeBook.Domain.Repositorys.Code;

namespace Unity.Test.Utils.Repository.Code;

public class CodeWriteOnlyRepositoryBuilder
{
    private static CodeWriteOnlyRepositoryBuilder _instance;
    private readonly Mock<ICodeWriteOnlyRepository> _repository;

    private CodeWriteOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<ICodeWriteOnlyRepository>();
    }

    public static CodeWriteOnlyRepositoryBuilder Instance()
    {
        _instance = new CodeWriteOnlyRepositoryBuilder();
        return _instance;
    }
    public ICodeWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}