using AutoMapper;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositorys.Connection;
using MyRecipeBook.Domain.Repositorys.Recipe;

namespace MyRecipeBook.Application.UseCases.Connection.Recover;

public class RecoverAllConnectionsUseCase : IRecoverAllConnectionsUseCase
{
    private readonly IUserLogged _userLogged;
    private readonly IConnectionReadOnlyRepository _connectionReadRepository;
    private readonly IRecipeReadOnlyRepository _recipeReadRepository;
    private readonly IMapper _mapper;

    public RecoverAllConnectionsUseCase(IUserLogged userLogged,
        IConnectionReadOnlyRepository connectionReadRepository,
        IRecipeReadOnlyRepository recipeReadRepository,
        IMapper mapper)
    {
        _userLogged = userLogged;
        _connectionReadRepository = connectionReadRepository;
        _recipeReadRepository = recipeReadRepository;
        _mapper = mapper;
    }

    public async Task<ResponseUserConnectionsJson> Execute()
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();

        var connections = await _connectionReadRepository.RecoverConnectedUsers(userLogged.Id);

        var tasks = connections.Select(async user =>
        {
            var recipeQuantity = await _recipeReadRepository.RecipeQuantity(user.Id);

            var userJson = _mapper.Map<ResponseUserConnectedJson>(user);
            userJson.RecipesQuantity = recipeQuantity;

            return userJson;
        });

        return new ResponseUserConnectionsJson
        {
            Users = await Task.WhenAll(tasks)
        };
    }
}
