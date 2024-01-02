using Moq;
using MyRecipeBook.Domain.Repositorys.Recipe;

namespace Unity.Test.Utils.Repository.Recipe;

public class RecipeWriteOnlyRepositoryBuilder
{
    private static RecipeWriteOnlyRepositoryBuilder _instance;
    private readonly Mock<IRecipeWriteOnlyRepository> _repository;

    private RecipeWriteOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IRecipeWriteOnlyRepository>();
    }

    public static RecipeWriteOnlyRepositoryBuilder Instance()
    {
        _instance = new RecipeWriteOnlyRepositoryBuilder();
        return _instance;
    }
    public IRecipeWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}
