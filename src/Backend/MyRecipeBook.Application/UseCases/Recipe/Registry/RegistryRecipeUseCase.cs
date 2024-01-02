using AutoMapper;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Domain.Repositorys.Recipe;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe.Registry;

public class RegistryRecipeUseCase : IRegistryRecipeUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IUserLogged _userLogged;
    private readonly IRecipeWriteOnlyRepository _recipeWriteRepository;

    public RegistryRecipeUseCase(IMapper mapper, 
        IUnityOfWork unityOfWork, 
        IUserLogged userLogged, 
        IRecipeWriteOnlyRepository recipeWriteRepository)
    {
        _mapper = mapper;
        _unityOfWork = unityOfWork;
        _userLogged = userLogged;
        _recipeWriteRepository = recipeWriteRepository;
    }

    public async Task<Communication.Response.ResponseRecipeJson> Execute(Communication.Request.RequestRecipeJson request)
    {
        Validate(request);

        var loggedUser = await _userLogged.RecoverUserLoggedToken();

        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = loggedUser.Id;

        await _recipeWriteRepository.Registry(recipe);

        await _unityOfWork.Commit();

        return _mapper.Map<Communication.Response.ResponseRecipeJson>(recipe);
    }

    private static void Validate(Communication.Request.RequestRecipeJson request)
    {
        var validator = new RegistryRecipeValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var erroMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErroException(erroMessages);
        }
    }
}
