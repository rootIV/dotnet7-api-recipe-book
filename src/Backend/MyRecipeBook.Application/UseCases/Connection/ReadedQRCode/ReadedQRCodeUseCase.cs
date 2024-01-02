using HashidsNet;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositorys.Code;
using MyRecipeBook.Domain.Repositorys.Connection;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Connection.ReadedQRCode;

public class ReadedQRCodeUseCase : IReadedQRCodeUseCase
{
    private readonly ICodeReadOnlyRepository _codeReadRepository;
    private readonly IUserLogged _userLogged;
    private readonly IConnectionReadOnlyRepository _connectionReadRepository;
    private readonly IHashids _hashIds;

    public ReadedQRCodeUseCase(ICodeReadOnlyRepository codeReadRepository, 
        IUserLogged userLogged, 
        IConnectionReadOnlyRepository connectionReadRepository,
        IHashids hashIds)
    {
        _codeReadRepository = codeReadRepository;
        _userLogged = userLogged;
        _connectionReadRepository = connectionReadRepository;
        _hashIds = hashIds;
    }

    public async Task<(ResponseUserConnectionJson ConnectingUser, string QRCodeGeneratorUserId)> Execute(string connectionCode)
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();
        var code = await _codeReadRepository.RecoverEntitieCode(connectionCode);

        await Validate(code, userLogged);

        var ConnectingUser = new ResponseUserConnectionJson
        {
            Id = _hashIds.EncodeLong(userLogged.Id),
            Name = userLogged.Name    
        };

        return (ConnectingUser, _hashIds.EncodeLong(code.UserId));
    }

    private async Task Validate(Domain.Entities.Codes code, Domain.Entities.User userLogged)
    {
        if(code is null)
        {
            throw new MyRecipeBookException("");
        }

        if (code.UserId == userLogged.Id)
        {
            throw new MyRecipeBookException("");
        }

        var existsConnection = await _connectionReadRepository.ExistsConnection(code.UserId, userLogged.Id);

        if (existsConnection)
        {
            throw new MyRecipeBookException("");
        }
    }
}
