using Moq;
using MyRecipeBook.Domain.Repositorys.Recipe;

namespace Unity.Test.Utils.Repository.Recipe;

public class RecipeReadOnlyRepositoryBuilder
{
    private static RecipeReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IRecipeReadOnlyRepository> _repository;

    private RecipeReadOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IRecipeReadOnlyRepository>();
    }

    public static RecipeReadOnlyRepositoryBuilder Instance()
    {
        _instance = new RecipeReadOnlyRepositoryBuilder();
        return _instance;
    }
    public RecipeReadOnlyRepositoryBuilder RecipeQuantity(int recipeQuantity)
    {
        _repository.Setup(r => r.RecipeQuantity(It.IsAny<long>())).ReturnsAsync(recipeQuantity);

        return this;
    }
    public RecipeReadOnlyRepositoryBuilder RecoverUserAllRecipes(MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        _repository.Setup(r => r.RecoverUserAllRecipes(recipe.UserId)).ReturnsAsync(new List<MyRecipeBook.Domain.Entities.Recipe> { recipe });

        return this;
    }
    public RecipeReadOnlyRepositoryBuilder RecoverRecipeById(MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        _repository.Setup(r => r.RecoverRecipeById(recipe.Id)).ReturnsAsync(recipe);

        return this;
    }
    public IRecipeReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
