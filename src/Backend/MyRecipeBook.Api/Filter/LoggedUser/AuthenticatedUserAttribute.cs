using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Application.Services.Token;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositorys.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Api.Filter.LoggedUser;

public class AuthenticatedUserAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly TokenController _tokenController;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public AuthenticatedUserAttribute(TokenController tokenController, 
        IUserReadOnlyRepository userReadOnlyRepository)
    {
        _tokenController = tokenController;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenInRequest(context);
            var email = _tokenController.RecoverEmailOnToken(token);

            var user = await _userReadOnlyRepository.RecoverUserByEmail(email) ??
                throw new MyRecipeBookException(string.Empty);
        }
        catch (SecurityTokenExpiredException)
        {
            ExpiredToken(context);
        }
        catch
        {
            UnauthorizedUserToken(context);
        }
    }

    private static string TokenInRequest(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authorization))
            throw new MyRecipeBookException(string.Empty);

        return authorization["Bearer".Length..].Trim();
    }
    private static void ExpiredToken(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ResponseErroJson(ErroMessagesResource.Token_Expired));
    }
    private static void UnauthorizedUserToken(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ResponseErroJson(ErroMessagesResource.User_Unauthorized));
    }
}
