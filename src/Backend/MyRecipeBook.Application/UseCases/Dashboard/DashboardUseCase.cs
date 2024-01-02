using AutoMapper;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositorys.Connection;
using MyRecipeBook.Domain.Repositorys.Recipe;
using System.Linq;

namespace MyRecipeBook.Application.UseCases.Dashboard;

public class DashboardUseCase : IDashboardUseCase
{
    private readonly IRecipeReadOnlyRepository _recipeReadRepository;
    private readonly IConnectionReadOnlyRepository _connectionReadRepository;
    private readonly IUserLogged _userLogged;
    private readonly IMapper _mapper;

    public DashboardUseCase(IRecipeReadOnlyRepository recipeReadRepository, 
        IUserLogged userLogged, 
        IMapper mapper,
        IConnectionReadOnlyRepository connectionReadRepository)
    {
        _recipeReadRepository = recipeReadRepository;
        _userLogged = userLogged;
        _mapper = mapper;
        _connectionReadRepository = connectionReadRepository;
    }

    public async Task<ResponseDashboardJson> Execute(RequestDashboardJson request)
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();

        var recipes = await _recipeReadRepository.RecoverUserAllRecipes(userLogged.Id);
        recipes = Filter(request, recipes);

        var usersConnectedRecipes = await UsersConnectedRecipes(request, userLogged);

        recipes = recipes.Concat(usersConnectedRecipes).ToList();

        return new ResponseDashboardJson
        {
            Recipes = _mapper.Map<List<ResponseDashboardRecipeJson>>(recipes)
        };
    }

    private async Task<IList<Domain.Entities.Recipe>> UsersConnectedRecipes(RequestDashboardJson request, Domain.Entities.User userLogged)
    {
        var connections = await _connectionReadRepository.RecoverConnectedUsers(userLogged.Id);

        var usersConnected = connections.Select(c => c.Id).ToList();

        var connectedUsersRecipe = await _recipeReadRepository.RecoverAllRecipesFromConnectedUsers(usersConnected);
        return Filter(request, connectedUsersRecipe);
    }
    private static IList<Domain.Entities.Recipe> Filter(RequestDashboardJson request, 
        IList<Domain.Entities.Recipe> recipes)
    {
        if (recipes is null)
            return new List<Domain.Entities.Recipe>();

        var filteredsRecipes = recipes;

        if (request.Category.HasValue)
            filteredsRecipes = recipes.Where(r => r.Category == (Domain.Enum.Category)request.Category.Value).ToList();

        if (!string.IsNullOrWhiteSpace(request.TitleOrIngredient))
            filteredsRecipes = recipes.Where(r => r.Title.CompareWithoutAccentsAndUpperCase(request.TitleOrIngredient) || r.Ingredients.Any(ingrediente => ingrediente.Product.CompareWithoutAccentsAndUpperCase(request.TitleOrIngredient))).ToList();

        return filteredsRecipes.OrderBy(r => r.Title).ToList();
    }
}
