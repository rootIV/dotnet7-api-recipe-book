using AutoMapper;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Domain.Repositorys.Recipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe.Update;

public class UpdateRecipeUseCase : IUpdateRecipeUseCase
{
    private readonly IRecipeUpdateOnlyRepository _recipeUpdateRepository;
    private readonly IUserLogged _userLogged;
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;

    public UpdateRecipeUseCase(IRecipeUpdateOnlyRepository recipeUpdateRepository, 
        IUserLogged userLogged, 
        IMapper mapper, 
        IUnityOfWork unityOfWork)
    {
        _recipeUpdateRepository = recipeUpdateRepository;
        _userLogged = userLogged;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
    }

    public async Task Execute(long recipeId, RequestRecipeJson request)
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();

        var recipe = await _recipeUpdateRepository.RecoverRecipeById(recipeId);

        Validate(userLogged, recipe, request);

        _mapper.Map(request, recipe);

        _recipeUpdateRepository.Update(recipe);

        await _unityOfWork.Commit();
    }

    public static void Validate(Domain.Entities.User userLogged, 
        Domain.Entities.Recipe recipe, 
        RequestRecipeJson request)
    {
        if (recipe is null || recipe.UserId != userLogged.Id)
            throw new ValidationErroException(new List<string> { ErroMessagesResource.User_Recipe_Not_Found });

        var validator = new UpdateRecipeValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var erroMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErroException(erroMessages);
        }
    }
}
