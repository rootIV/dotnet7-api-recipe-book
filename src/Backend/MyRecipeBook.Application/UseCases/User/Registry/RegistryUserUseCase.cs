using AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.Services.Token;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Domain.Repositorys.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.User.Registry;

public class RegistryUserUseCase : IRegistryUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadRepository;
    private readonly IUserWriteOnlyRepository _userWriteRepository;
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;
    private readonly EncPassword _encPassword;
    private readonly TokenController _tokenController;

    public RegistryUserUseCase(IUserReadOnlyRepository userReadRepository, 
        IUserWriteOnlyRepository userWriteRepository,
        IMapper mapper, 
        IUnityOfWork unityOfWork,
        EncPassword encPassword, 
        TokenController tokenController)
    {
        _userReadRepository = userReadRepository;
        _userWriteRepository = userWriteRepository;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
        _encPassword = encPassword;
        _tokenController = tokenController;
    }

    public async Task<Communication.Response.ResponseRegistryUserJson> Execute(Communication.Request.RequestRegistryUserJson request)
    {
        await Validate(request);

        var entitie = _mapper.Map<Domain.Entities.User>(request);
        entitie.Password = _encPassword.Encrypt(request.Password);

        await _userWriteRepository.Add(entitie);
        await _unityOfWork.Commit();

        var token = _tokenController.GenerateToken(entitie.Email);

        return new Communication.Response.ResponseRegistryUserJson
        {
            Token = token
        };
    }

    private async Task Validate(Communication.Request.RequestRegistryUserJson request)
    {
        var validator = new RegistryUserValidator();
        var result = validator.Validate(request);

        var userEmailAlreayExists = await _userReadRepository.ExistUserEmail(request.Email);
        if(userEmailAlreayExists)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ErroMessagesResource.User_Email_Already_Taken));

        if (!result.IsValid)
        {
            var erroMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErroException(erroMessages);
        }
    }
}
