using AutoMapper;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositorys.Connection;
using MyRecipeBook.Domain.Repositorys.Recipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe.RecoverById;

public class RecoverRecipeByIdUseCase : IRecoverRecipeByIdUseCase
{
    private readonly IRecipeReadOnlyRepository _recipeReadRepository;
    private readonly IUserLogged _userLogged;
    private readonly IMapper _mapper;
    private readonly IConnectionReadOnlyRepository _connectionReadRepository;

    public RecoverRecipeByIdUseCase(IRecipeReadOnlyRepository recipeReadRepository, 
        IUserLogged userLogged, 
        IMapper mapper,
        IConnectionReadOnlyRepository connectionReadRepository)
    {
        _recipeReadRepository = recipeReadRepository;
        _userLogged = userLogged;
        _mapper = mapper;
        _connectionReadRepository = connectionReadRepository;
    }

    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();

        var recipe = await _recipeReadRepository.RecoverRecipeById(recipeId);

        await Validate(userLogged, recipe);

        return _mapper.Map<ResponseRecipeJson>(recipe);
    }

    private async Task Validate(Domain.Entities.User userLogged, Domain.Entities.Recipe recipe)
    {
        var connectedUsers = await _connectionReadRepository.RecoverConnectedUsers(userLogged.Id);

        if (recipe is null || (recipe.UserId != userLogged.Id && !connectedUsers.Any(user => user.Id == recipe.UserId))) 
            throw new ValidationErroException(new List<string> { ErroMessagesResource.User_Recipe_Not_Found });
    }
}
