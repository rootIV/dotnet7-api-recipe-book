using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.Services.Token;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositorys.User;

namespace MyRecipeBook.Application.Services.UserLogged;

public class UserLogged : IUserLogged
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenController _tokenController;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;


    public UserLogged(IHttpContextAccessor httpContextAcessor,
        TokenController tokenController,
        IUserReadOnlyRepository userReadOnlyRepository)
    {
        _httpContextAccessor = httpContextAcessor;
        _tokenController = tokenController;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<User> RecoverUserLoggedToken()
    {
        var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        var token = authorization["Bearer".Length..].Trim();
        var emailUser = _tokenController.RecoverEmailOnToken(token);

        var user = await _userReadOnlyRepository.RecoverUserByEmail(emailUser);

        return user;
    }
}
