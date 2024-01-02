using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Domain.Repositorys.Connection;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Connection.Remove;

public class RemoveConnectionsUseCase : IRemoveConnectionsUseCase
{
    private readonly IUserLogged _userLogged;
    private readonly IConnectionReadOnlyRepository _connectionReadRepository;
    private readonly IConnectionWriteOnlyRepository _connectionWriteRepository;
    private readonly IUnityOfWork _unityOfWork;

    public RemoveConnectionsUseCase(IUserLogged userLogged,
        IConnectionReadOnlyRepository connectionReadRepository,
        IConnectionWriteOnlyRepository connectionWriteRepository,
        IUnityOfWork unityOfWork)
    {
        _userLogged = userLogged;
        _connectionReadRepository = connectionReadRepository;
        _connectionWriteRepository = connectionWriteRepository;
        _unityOfWork = unityOfWork;
    }

    public async Task Execute(long connectedUserIdToRemove)
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();

        var connectedUsers = await _connectionReadRepository.RecoverConnectedUsers(userLogged.Id);

        Validate(connectedUsers, connectedUserIdToRemove);

        await _connectionWriteRepository.RemoveConnection(userLogged.Id ,connectedUserIdToRemove);

        await _unityOfWork.Commit();

    }
    public static void Validate(IList<Domain.Entities.User> connectedUsers, long connectedUserIdToRemove)
    {

        if (!connectedUsers.Any(user => user.Id == connectedUserIdToRemove))
            throw new ValidationErroException(new List<string> { ErroMessagesResource.User_Not_Found });
    }
}
