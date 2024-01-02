using HashidsNet;
using MyRecipeBook.Domain.Repositorys.Code;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Domain.Repositorys.Connection;

namespace MyRecipeBook.Application.UseCases.Connection.AcceptConnection;

public class AcceptConnectionUseCase : IAcceptConnectionUseCase
{
    private readonly ICodeWriteOnlyRepository _codeWriteRepository;
    private readonly IUserLogged _userLogged;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IHashids _hashids;
    private readonly IConnectionWriteOnlyRepository _connectionWriteRepository;

    public AcceptConnectionUseCase(ICodeWriteOnlyRepository codeWriteRepository,
        IUserLogged userLogged,
        IUnityOfWork unityOfWork,
        IHashids hashids,
        IConnectionWriteOnlyRepository connectionWriteRepository)
    {
        _codeWriteRepository = codeWriteRepository;
        _userLogged = userLogged;
        _unityOfWork = unityOfWork;
        _hashids = hashids;
        _connectionWriteRepository = connectionWriteRepository;
    }

    public async Task<string> Execute(string userToConnectId)
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();

        await _codeWriteRepository.Delete(userLogged.Id);

        var qrCodeReaderConnectionId = _hashids.DecodeLong(userToConnectId).First();

        await _connectionWriteRepository.Registry(new Domain.Entities.Connection
        {
            UserId = userLogged.Id,
            ConnectedWithUserId = qrCodeReaderConnectionId
        });

        await _connectionWriteRepository.Registry(new Domain.Entities.Connection
        {
            UserId = qrCodeReaderConnectionId,
            ConnectedWithUserId = userLogged.Id
        });

        await _unityOfWork.Commit();

        return _hashids.EncodeLong(userLogged.Id);
    }
}
