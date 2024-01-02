using Microsoft.AspNetCore.Authorization;
using MyRecipeBook.Application.Services.Token;
using MyRecipeBook.Domain.Repositorys.User;

namespace MyRecipeBook.Api.Filter.LoggedUser;

public class LoggedUserHandler : AuthorizationHandler<LoggedUserRequirement>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TokenController _tokenController;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;


    public LoggedUserHandler(IHttpContextAccessor contextAccessor,
        TokenController tokenController,
        IUserReadOnlyRepository userReadOnlyRepository)
    {
        _contextAccessor = contextAccessor;
        _tokenController = tokenController;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        LoggedUserRequirement requirement)
    {
        try
        {
            var authorization = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authorization))
            {
                context.Fail();
                return;
            }

            var token = authorization["Bearer".Length..].Trim();
            var emailUser = _tokenController.RecoverEmailOnToken(token);

            var user = await _userReadOnlyRepository.RecoverUserByEmail(emailUser);

            if (user is null)
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
        catch
        {
            context.Fail();
        }
    }
}
