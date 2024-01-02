using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.Services.Token;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositorys.User;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class LoginUserUseCase : ILoginUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadRepository;
    private readonly EncPassword _encPassword;
    private readonly TokenController _tokenController;

    public LoginUserUseCase(IUserReadOnlyRepository userReadRepository, 
        EncPassword encPassword, 
        TokenController tokenController)
    {
        _userReadRepository = userReadRepository;
        _encPassword = encPassword;
        _tokenController = tokenController;
    }

    public async Task<ResponseLoginJson> Execute(RequestLoginJson request)
    {
        Validate(request);

        var encPass = _encPassword.Encrypt(request.Password);
        var user = await _userReadRepository.RecoverUserByEmailPass(request.Email, encPass) ?? throw new InvalidLoginException();

        return new ResponseLoginJson
        {
            Name = user.Name,
            Token = _tokenController.GenerateToken(user.Email)
        };
    }

    private static void Validate(RequestLoginJson request)
    {
        var validator = new LoginUserValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var erroMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErroException(erroMessages);
        }
    }
}
