using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Domain.Repositorys.Recipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe.Delete;

public class DeleteRecipeUseCase : IDeleteRecipeUseCase
{
    private readonly IRecipeReadOnlyRepository _recipeReadRepository;
    private readonly IRecipeWriteOnlyRepository _recipeWriteRepository;
    private readonly IUserLogged _userLogged;
    private readonly IUnityOfWork _unityOfWork;

    public DeleteRecipeUseCase(IRecipeReadOnlyRepository recipeReadRepository, 
        IRecipeWriteOnlyRepository recipeWriteRepository, 
        IUserLogged userLogged,
        IUnityOfWork unityOfWork)
    {
        _recipeReadRepository = recipeReadRepository;
        _recipeWriteRepository = recipeWriteRepository;
        _userLogged = userLogged;
        _unityOfWork = unityOfWork;
    }

    public async Task Execute(long recipeId)
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();

        var recipe = await _recipeReadRepository.RecoverRecipeById(recipeId);

        Validate(userLogged, recipe);

        await _recipeWriteRepository.Delete(recipeId);

        await _unityOfWork.Commit();
    }

    public static void Validate(Domain.Entities.User userLogged, Domain.Entities.Recipe recipe)
    {
        if (recipe is null || recipe.UserId != userLogged.Id)
            throw new ValidationErroException(new List<string> { ErroMessagesResource.User_Recipe_Not_Found });
    }
}
