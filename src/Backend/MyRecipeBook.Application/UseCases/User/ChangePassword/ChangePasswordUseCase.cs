using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Domain.Repositorys.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly IUserUpdateOnlyRepository _updateRepository;
    private readonly IUserLogged _userLogged;
    private readonly IUnityOfWork _unityOfWork;
    private readonly EncPassword _encPassword;

    public ChangePasswordUseCase(IUserUpdateOnlyRepository updateRepository, 
        IUserLogged userLogged, 
        IUnityOfWork unityOfWork, 
        EncPassword encPassword)
    {
        _updateRepository = updateRepository;
        _userLogged = userLogged;
        _encPassword = encPassword;
        _unityOfWork = unityOfWork;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();
        var user = await _updateRepository.RecoverUserById(userLogged.Id);

        Validate(request, user);

        user.Password = _encPassword.Encrypt(request.NewPassword);

        _updateRepository.Update(user);

        await _unityOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User user)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var actualPasswordEncrypted = _encPassword.Encrypt(request.ActualPassword);
        if (!user.Password.Equals(actualPasswordEncrypted))
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("actualPassword", ErroMessagesResource.User_Password_Invalid));

        if (!result.IsValid)
        {
            var erroMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErroException(erroMessages);
        }
    }
}
