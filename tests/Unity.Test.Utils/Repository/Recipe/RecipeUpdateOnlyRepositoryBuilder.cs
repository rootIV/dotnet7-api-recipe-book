using Moq;
using MyRecipeBook.Domain.Repositorys.Recipe;

namespace Unity.Test.Utils.Repository.Recipe;

public class RecipeUpdateOnlyRepositoryBuilder
{
    private static RecipeUpdateOnlyRepositoryBuilder _instance;
    private readonly Mock<IRecipeUpdateOnlyRepository> _repository;

    private RecipeUpdateOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IRecipeUpdateOnlyRepository>();
    }

    public static RecipeUpdateOnlyRepositoryBuilder Instance()
    {
        _instance = new RecipeUpdateOnlyRepositoryBuilder();
        return _instance;
    }
    public RecipeUpdateOnlyRepositoryBuilder RecoverById(MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        _repository.Setup(r => r.RecoverRecipeById(recipe.Id)).ReturnsAsync(recipe);

        return this;
    }
    public IRecipeUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }
}
